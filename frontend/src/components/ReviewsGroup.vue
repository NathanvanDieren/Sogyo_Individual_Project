<script setup lang="ts">
import { apiDelete, apiGet } from "../services/api.ts";
import ErrorBox from "../components/ErrorBox.vue";
import ReviewForm from "./ReviewForm.vue";
import { ReviewListDto } from "../dtos/ReviewDtos.ts"
import {computed, ref} from 'vue'
import { GroupListDto } from "../dtos/GroupDtos.ts";
import TagFilter from "./TagFilter.vue";

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
  name: string
  members: string
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
    itemType: review.itemtype || review.itemType || '',
    groupsguids: review.groupsguids || review.groupIds || []
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

const activeTags = ref<string[]>([])

const filteredItems = computed(() => {
  const rawData = reviewList.value

  const allReviews = Array.isArray(rawData)
      ? rawData
      : rawData?.reviews || []

  if (activeTags.value.length === 0 || activeTags.value[0] === '') {
    return allReviews
  }

  return allReviews.filter(item => activeTags.value.includes(item.name))
})
</script>

<template>
  <ErrorBox v-if="errorText" :error-text="errorText" :error-type="errorType" @clear-error="errorText = ''" />

  <ReviewForm
      v-if="showReviewModal"
      :available-groups="groupList?.groups || []"
      :review-to-edit="selectedReview"
      @close="showReviewModal = false"
      @success="GetReviews"
  />

  <div v-else-if="reviewList" class="reviews-container">
    <button @click="openCreateModal" class="create-main-btn">+ Schrijf Review</button>
        <div v-if="reviewList.reviews.length > 0">
          <p class="total-count">Reviews: {{ reviewList.totalCount }}</p>
          <TagFilter id="tagFilter" :items="reviewList.reviews || reviewList" v-model="activeTags" />
          <ul class="reviews-list">
            <li
                v-for="review in filteredItems"
                :key="review.id"
                class="review-item"
            >
              <div class="review-header">
                <h3>{{ review.title }}</h3>

                <div class="tags">
                  <div class="creator">
                    <small class="creatortext">
                      {{ review.name || 'Onbekend' }}
                    </small>
                  </div>

                  <div class="itemtype">
                    <small class="itemtext">
                      {{ review.itemtype || 'Onbekend' }}
                    </small>
                  </div>
                </div>
              </div>

              <div class="rating-field">
                <div class="stars-display">
                <span
                    v-for="star in [1, 2, 3, 4, 5]"
                    :key="star"
                    class="star"
                    :class="{
                    'active': review.rating >= star,
                    'half': review.rating === star - 0.5
                  }"
                >
                  ★
                </span>
                </div>
              </div>

              <small class="description-text">{{ review.description }}</small>

              <div v-if="review.isCreator" class="actions-container">
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
.reviews-container {
  max-width: 500px;
  margin: 40px auto;
  padding: 20px;
  font-family: sans-serif;
}

.total-count {
  text-align: left;
  font-weight: bold;
  color: var(--text-secondary);
  margin-bottom: 20px;
}
#tagFilter {
  padding: 10px 0px;
}

.reviews-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.review-item {
  position: relative;
  background-color: var(--bg-secondary);
  border: 2px solid var(--border-color);
  border-radius: 12px;
  padding: 20px;
  cursor: pointer;
  transition: all 0.2s ease-in-out;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
}

.review-item:active {
  transform: translateY(0);
  background-color: var(--bg-input);
}

.review-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
}

.review-item h3 {
  margin: 0;
  color: var(--text-primary);
  max-width: 70%;
}

.tags{
  display: flex;
  justify-content: flex-end
}

.itemtype {
  padding: 0 5px;
}

.itemtext {
  font-size: 0.8rem;
  color: var(--text-primary);
  background-color: var(--bg-tag-green);
  padding: 4px 8px;
  border-radius: 6px;
  font-weight: 500;
}

.creator {
  padding: 0 5px;
}

.creatortext {
  font-size: 0.8rem;
  color: var(--text-primary);
  background-color: var(--bg-tag-blue);
  padding: 4px 8px;
  border-radius: 6px;
  font-weight: 800;
}

.description-text {
  display: block;
  color: var(--text-secondary);
  font-weight: 600;
  margin-bottom: 6px;
}

.author-text {
  display: block;
  color: var(--text-secondary);
  font-weight: 600;
  margin-bottom: 16px;
}

.create-main-btn {
  background-color: var(--accent-primary);
  color: var(--text-white);
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
  background-color: var(--accent-primary-hover);
}

.actions-container {
  display: flex;
  gap: 8px;
  margin-top: 12px;
  border-top: 1px solid var(--bg-input);
  padding-top: 12px;
}

.editbutton {
  font-size: 0.8rem;
  color: var(--text-primary);
  background-color: var(--accent-success);
  opacity: 0.8;
  padding: 4px 12px;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  border: none;
}

.deletebutton {
  font-size: 0.8rem;
  color: var(--text-white);
  background-color: var(--accent-danger);
  opacity: 0.8;
  padding: 4px 12px;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  border: none;
}

.editbutton:hover, .deletebutton:hover {
  opacity: 1;
}

.rating-field {
  margin-bottom: 12px;
}

.rating-label {
  display: block;
  font-weight: 500;
  color: var(--text-primary);
  margin-bottom: 4px;
}

.stars-display {
  display: inline-flex;
  gap: 2px;
  line-height: 1;
}

.stars-display .star {
  display: inline-block;
  position: relative;
  font-size: 30px;
  width: 1.1em;
  height: 1.1em;
  color: var(--border-color);
  text-align: left;
}

.stars-display .star.active {
  color: var(--star-filled);
}

.stars-display .star.half {
  color: var(--star-empty);
}

.stars-display .star.half:before {
  content: "★";
  position: absolute;
  left: 0;
  top: 0;
  width: 41%;
  overflow: hidden;
  color: var(--star-filled);
  white-space: nowrap;
}
</style>
