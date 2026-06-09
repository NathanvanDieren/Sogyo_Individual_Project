<script setup lang="ts">
import { apiDelete, apiGet } from "../services/api.ts";
import ErrorBox from "../components/ErrorBox.vue";
import ReviewForm from "./ReviewForm.vue"; // 1. Importeer de nieuwe ReviewForm
import { ReviewListDto } from "../dtos/ReviewDtos.ts"
import { ref } from 'vue'
import { GroupListDto } from "../dtos/GroupDtos.ts";

const errorType = ref<string>('')
const errorText = ref<string>('')

const showReviewModal = ref(false)
const selectedReview = ref<any>(null)

const reviewList = ref<ReviewListDto | null>(null)
const groupList = ref<GroupListDto | null>(null)

defineExpose({
  GetReviews
})

const props = defineProps<{
  id: string
}>()

GetReviews().then(() => {
  GetGroups()
})

async function GetReviews() {
  try {
    reviewList.value = await apiGet<ReviewListDto>(`/api/review/getreviews/${props.id}`)
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}

async function GetGroups() {
  try {
    groupList.value = await apiGet<GroupListDto>('/api/group/getgroups')
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}


function openCreateModal() {
  selectedReview.value = null
  showReviewModal.value = true
}


function editReview(review: any) {

  selectedReview.value = {
    id: review.id,
    title: review.title,
    description: review.description,
    rating: review.rating,
    itemType: review.itemtype || review.itemType,
    groupsguids: review.groupsguids || []
  }
  showReviewModal.value = true
}

async function deleteReview(reviewId: string) {
  const bevestigd = confirm("Weet je zeker dat je deze review wilt verwijderen?");
  if (!bevestigd) return;

  try {
    await apiDelete(`/api/review/delete/${reviewId}`)
    await GetReviews() // Ververs de lijst direct
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Kon de review niet verwijderen.'
  }
}
</script>

<template>
  <ErrorBox v-if="errorText" :error-text="errorText" :error-type="errorType" />

  <ReviewForm
      v-if="showReviewModal"
      :available-groups="groupList?.groups || []"
      :review-to-edit="selectedReview"
      @close="showReviewModal = false"
      @success="GetReviews"
  />

  <div v-else-if="reviewList" class="groups-container">
    <button @click="openCreateModal" class="create-main-btn">+ Schrijf Review</button>

    <div v-if="reviewList.reviews.length > 0">
      <p class="total-count">Reviews: {{ reviewList.totalCount }}</p>

      <ul class="groups-list">
        <li
            v-for="review in reviewList.reviews"
            :key="review.id"
            class="group-item"
        >
          <div class="review-header">
            <h3>{{ review.title }}</h3>

            <div class="itemtype">
              <small class="itemtext">
                {{ review.itemtype || 'Onbekend' }}
              </small>
            </div>
          </div>

          <div class="rating-field">
            <label class="block font-medium text-gray-700 mb-1">Beoordeling:</label>

            <div class="stars-display">
                <span
                    v-for="star in [1, 2, 3, 4, 5]"
                    :key="star"
                    class="star"
                    :class="{ 'active': star <= review.rating }"
                >
                  ★
                </span>
            </div>
          </div>

          <small class="members-title">Beschrijving: {{ review.description }}</small>
          <small class="members-title mb-4">Geschreven door: {{ review.name }}</small>

          <div class="actions-container">
            <button @click.stop="editReview(review)" class="editbutton">Edit</button>
            <button @click.stop="deleteReview(review.id)" class="deletebutton">Delete</button>
          </div>
        </li>
      </ul>
    </div>

    <p v-else>Nog geen reviews geschreven</p>
  </div>
  <p v-else>Reviews laden...</p>
</template>

<style scoped>
.groups-container {
  max-width: 500px;
  margin: 40px auto;
  padding: 20px;
  font-family: sans-serif;
}

.total-count {
  text-align: left;
  font-weight: bold;
  color: #555;
  margin-bottom: 20px;
}

.groups-list, .members-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.groups-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.group-item {
  position: relative;
  background-color: #ffffff;
  border: 2px solid #e0e0e0;
  border-radius: 12px;
  padding: 20px;
  cursor: pointer;
  transition: all 0.2s ease-in-out;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
}


.group-item:active {
  transform: translateY(0);
  background-color: #f8fafc;
}

.review-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 8px;
}

.group-item h3 {
  margin: 0;
  color: #1e293b;
  max-width: 70%; /* Voorkomt dat een hele lange titel door je label heen loopt */
}

.itemtype {
  position: absolute;
  top: 20px;
  right: 20px;
}

.itemtext {
  font-size: 0.8rem;
  color: black;
  background-color: mediumseagreen;
  padding: 4px 8px;
  border-radius: 6px;
  font-weight: 500;
  white-space: nowrap;
}

.members-title {
  display: block;
  color: #64748b;
  font-weight: 600;
  margin-bottom: 6px;
}

.members-list li {
  font-size: 0.9rem;
  color: #334155;
  padding: 4px 0;
  border-bottom: 1px dashed #f1f5f9;
}

.members-list li:last-child {
  border-bottom: none;
}
.create-main-btn {
  background-color: #3b82f6;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 8px;
  font-weight: bold;
  cursor: pointer;
  margin-bottom: 20px;
  display: block;
  transition: background-color 0.2s;
}

.create-main-btn:hover {
  background-color: #2563eb;
}

.actions-container {
  display: flex;
  gap: 8px;
  margin-top: 12px;
  border-top: 1px solid #f1f5f9;
  padding-top: 12px;
}

.editbutton {
  font-size: 0.8rem;
  color: black;
  background-color: mediumseagreen;
  opacity: 0.8;
  padding: 4px 12px;
  border-radius: 6px;
  font-weight: 500;
  cursor: pointer;
  border: none;
}

.deletebutton {
  font-size: 0.8rem;
  color: white;
  background-color: red;
  opacity: 0.8;
  padding: 4px 12px;
  border-radius: 6px;
  font-weight: 500;
  cursor: pointer;
  border: none;
}

.editbutton:hover, .deletebutton:hover {
  opacity: 1;
}

.mb-4 {
  margin-bottom: 16px;
}

.stars-display {
  display: flex;
  gap: 4px;
}

.stars-display .star {
  font-size: 2.5rem;
  color: #ccc;
  user-select: none;
}


.stars-display .star.active {
  color: #ffc107;
}
</style>