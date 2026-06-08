<script setup lang="ts">
import {apiGet} from "../services/api.ts";
import ErrorBox from "./ErrorBox.vue";
import {ReviewListDto} from "../dtos/ReviewDtos.ts"
import { ref } from 'vue'


const errorType = ref<string>('')
const errorText = ref<string>('')

const reviewList = ref<ReviewListDto | null>(null)
defineExpose({
  GetReviews
})

GetReviews().then(() => {
  if (reviewList.value?.reviews) {
    reviewList.value.reviews.forEach(group => {
      console.log(group.title)
    })
  }
})

async function GetReviews() {
  try {
    reviewList.value = await apiGet<ReviewListDto>('/api/review/getreviews')
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
        <h3>{{ review.title }}</h3>

        <small class="members-title">Rating: {{review.rating}} ⭐</small>
        <small class="members-title">Description: {{review.description}}</small>
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
  background-color: #ffffff;
  border: 2px solid #e0e0e0;
  border-radius: 12px;
  padding: 20px;
  cursor: pointer;
  transition: all 0.2s ease-in-out;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
}


.group-item:hover {
  border-color: #3b82f6;
  transform: translateY(-2px);
  box-shadow: 0 6px 12px rgba(59, 130, 246, 0.15);
}

.group-item:active {
  transform: translateY(0);
  background-color: #f8fafc;
}

.group-item h3 {
  margin-top: 0;
  margin-bottom: 8px;
  color: #1e293b;
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