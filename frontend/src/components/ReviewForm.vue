<script setup lang="ts">
import ErrorBox from "./ErrorBox.vue";
import { ref, onMounted, watch } from 'vue'
import { apiGet, apiPost } from "../services/api.ts";
import { ItemTypeDto } from "../dtos/ReviewDtos.ts";

interface ReviewEditData {
  id: string
  title: string
  description: string
  rating: number
  itemType: string
  groupsguids: string[]
}

const props = defineProps<{
  availableGroups: any[]
  reviewToEdit?: ReviewEditData | null
}>()

const emit = defineEmits(['close', 'success'])

const title = ref<string>('')
const description = ref<string>('')
const rating = ref<number>(0)
const itemType = ref<string>('')
const selectedGroupsGuids = ref<string[]>([])

const errorMessage = ref<string>('')
const errorType = ref<string>('')

const itemTypeList = ref<ItemTypeDto[]>([])

async function GetItemTypes() {
  try {
    itemTypeList.value = await apiGet<ItemTypeDto[]>('/api/review/getitemtypes')
  } catch (err: any) {
    errorMessage.value = err.message || 'Er is een fout opgetreden bij het ophalen van de types.'
  }
}

function initializeForm() {
  errorMessage.value = ''
  errorType.value = ''

  if (props.reviewToEdit) {
    title.value = props.reviewToEdit.title || ''
    description.value = props.reviewToEdit.description || ''
    rating.value = props.reviewToEdit.rating ?? 0
    itemType.value = props.reviewToEdit.itemType || ''
    selectedGroupsGuids.value = props.reviewToEdit.groupsguids ? [...props.reviewToEdit.groupsguids] : []
  } else {
    title.value = ''
    description.value = ''
    rating.value = 0
    itemType.value = ''
    selectedGroupsGuids.value = []
  }
}

onMounted(() => {
  GetItemTypes()
})

watch(
    () => props.reviewToEdit,
    () => {
      initializeForm()
    },
    { immediate: true }
)

async function SaveReviewAndClose() {
  errorMessage.value = ''
  errorType.value = ''

  interface SaveReviewRequest {
    title: string,
    rating: number,
    description: string,
    itemType: string,
    groupsguids: string[]
  }

  const data: SaveReviewRequest = {
    title: title.value,
    rating: rating.value,
    description: description.value,
    itemType: itemType.value,
    groupsguids: selectedGroupsGuids.value,
  }

  try {
    if (props.reviewToEdit) {
      await apiPost<SaveReviewRequest, any>(`/api/review/edit/${props.reviewToEdit.id}`, data)
    } else {
      await apiPost<SaveReviewRequest, any>('/api/review/create', data)
    }

    emit('success')
    emit('close')
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorMessage.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}

</script>

<template>
  <div class="modal-overlay" @click="emit('close')">
    <form class="modal-content" @click.stop @submit.prevent="SaveReviewAndClose">

      <h2 class="modal-title">
        {{ props.reviewToEdit ? 'Review bewerken' : 'Schrijf een Review' }}
      </h2>

      <label for="title" class="label">Titel</label>
      <input type="text" id="title" v-model="title" required class="inputField">

      <label for="description" class="label">Omschrijving</label>
      <input type="text" id="description" v-model="description" required class="inputField">

      <div class="rating-field">
        <label class="rating-label">Beoordeling:</label>

        <div class="stars">
          <template v-for="star in [5, 4, 3, 2, 1]" :key="star">
            <input
                type="radio"
                :id="'review-star-' + star"
                name="review-rating"
                :value="star"
                :checked="rating === star"
                @change="rating = star"
            />
            <label :for="'review-star-' + star" :title="star + ' sterren'"></label>
          </template>
        </div>
      </div>

      <label for="itemtypes" class="label">Type</label>
      <select name="itemtypes" id="itemtypes" v-model="itemType" required class="inputField">
        <option value="" disabled selected>Kies een type...</option>
        <option
            v-for="type in itemTypeList"
            :key="type.value"
            :value="type.value"
        >
          {{ type.name }}
        </option>
      </select>

      <div class="groups-selection" v-if="availableGroups.length > 0">
        <label class="label">Delen met groepen:</label>
        <div v-for="group in availableGroups" :key="group.id" class="checkbox-item">
          <input
              type="checkbox"
              :id="group.id"
              :value="group.id"
              v-model="selectedGroupsGuids"
          />
          <label :for="group.id" class="checkbox-label">{{ group.name }}</label>
        </div>
      </div>

      <ErrorBox v-if="errorMessage" :error-type="errorType" :error-text="errorMessage"></ErrorBox>

      <div class="form-actions">
        <button type="button" class="cancelButton" @click="emit('close')">Annuleren</button>
        <button type="submit" class="submitButton">
          {{ props.reviewToEdit ? 'Opslaan' : 'Post' }}
        </button>
      </div>
    </form>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 10000;
}

.modal-content {
  background: white;
  padding: 30px;
  border-radius: 8px;
  min-width: 350px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.2);
}

.modal-title {
  font-weight: bold;
  font-size: 1.5rem;
  margin-bottom: 16px;
}

.label {
  display: block;
  font-weight: 500;
  color: #374151;
  margin-bottom: 4px;
}

.inputField {
  width: 100%;
  background-color: #f9fafb;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  padding: 8px 12px;
  margin-bottom: 16px;
  box-sizing: border-box;
}

.inputField:focus {
  outline: none;
  border-color: #3b82f6;
}

.error {
  color: red;
  font-size: 14px;
}

.form-actions {
  display: flex;
  gap: 8px;
  margin-top: 16px;
}

.checkbox-item {
  display: flex;
  align-items: center;
  margin-bottom: 8px;
}

.checkbox-label {
  margin-left: 8px;
}

.groups-selection {
  margin-bottom: 16px;
}

.cancelButton {
  padding: 8px 16px;
  background-color: #64748b;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
}

.cancelButton:hover {
  background-color: #475569;
}

.submitButton {
  flex: 1;
  background-color: #3b82f6;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
}

.submitButton:hover {
  background-color: #2563eb;
}

.rating-field {
  margin-bottom: 15px;
}

.rating-label {
  display: block;
  font-weight: 500;
  color: #374151;
  margin-bottom: 4px;
}

.stars {
  display: flex;
  flex-direction: row-reverse;
  justify-content: flex-end;
}

.stars input {
  display: none;
}

.stars label {
  font-size: 2.5rem;
  color: #ccc;
  cursor: pointer;
  transition: color 0.15s ease-in-out;
  padding: 0 4px;
}

.stars label::before {
  content: '★';
}

.stars label:hover,
.stars label:hover ~ label {
  color: #ffca08;
}

.stars input:checked ~ label {
  color: #ffc107;
}

.stars input:checked ~ label:hover,
.stars input:checked ~ label:hover ~ label {
  color: #ffca08;
}
</style>
