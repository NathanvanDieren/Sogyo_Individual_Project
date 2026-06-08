<script setup lang="ts">
import { apiGet } from "../services/api.ts";
import ErrorBox  from "../components/ErrorBox.vue";
import { ReviewListDto } from "../dtos/ReviewDtos.ts"
import { ref } from 'vue'

const errorType = ref<string>('')
const errorText = ref<string>('')

const reviewList = ref<ReviewListDto | null>(null)

defineExpose({
  GetReviews
})

const props = defineProps<{
  id: string
}>()

GetReviews().then(() => {
  if (reviewList.value?.reviews) {
    reviewList.value.reviews.forEach(group => {
      console.log(group.title)
    })
  }
})

async function GetReviews() {
  try {
    reviewList.value = await apiGet<ReviewListDto>(`/api/review/getreviews/${props.id}`)
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}
</script>

<template>
  <ErrorBox v-if="errorText" :error-text="errorText" :error-type="errorType" />

  <div v-else-if="reviewList && reviewList.reviews.length > 0" class="groups-container">
    <p class="total-count">Reviews: {{ reviewList.totalCount }}</p>

    <ul class="groups-list">
      <li
          v-for="review in reviewList.reviews"
          :key="review.id"
          class="group-item"
          role="button"
          tabindex="0"
      >
        <div class="review-header">
          <h3>{{ review.title }}</h3>
          <div class="itemtype">
            <small class="itemtext">
              {{ review.itemtype|| 'Onbekend' }}
            </small>
          </div>
        </div>

        <small class="members-title">Beoordeling: {{ review.rating }} ⭐</small>
        <small class="members-title">Beschrijving: {{ review.description }}</small>
        <small class="members-title">Geschreven door: {{review.name}}</small>
      </li>
    </ul>
  </div>
  <p v-else-if="reviewList?.reviews?.length === 0">Nog geen reviews geschreven</p>
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
</style>