<script setup lang="ts">
import ErrorBox from "./ErrorBox.vue";
import { ref, onMounted } from 'vue'
import type { Ref } from 'vue'
import { apiGet, apiPost } from "../services/api.ts";
import { ItemTypeDto } from "../dtos/ReviewDtos.ts";


const title = ref<string>('')
const description = ref<string>('')
const rating = ref<number>(0)
const itemType = ref<string>('')

const errorMessage: Ref<string> = ref('')
const emit = defineEmits(['close', 'success'])


const itemTypeList = ref<ItemTypeDto[]>([])

async function GetItemTypes() {
  try {
    itemTypeList.value = await apiGet<ItemTypeDto[]>('/api/review/getitemtypes')
  } catch (err: any) {
    errorMessage.value = err.message || 'Er is een fout opgetreden bij het ophalen van de types.'
  }
}

onMounted(() => {
  GetItemTypes()
})

async function CreateReviewAndClose() {
  interface CreateReviewResponse {
    title: string,
    rating: number,
    description: string,
    itemType: string,
    groupsguids: string[]
  }

  const data: CreateReviewResponse = {
    title: title.value,
    rating: rating.value,
    description: description.value,
    itemType: itemType.value,
    groupsguids: []
  }

  try {
    await apiPost<CreateReviewResponse, any>('/api/review/create', data)
    emit('success')
    emit('close')
  } catch (err: any) {
    errorMessage.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}
</script>

<template>
  <div class="modal-overlay" @click="emit('close')">
    <form class="modal-content" @click.stop>
      <h2 class="font-bold text-2xl mb-4">Schrijf een Review</h2>

      <label for="title" class="label">Titel</label>
      <input type="text" id="title" v-model="title" required
             class="inputField">

      <label for="description" class="label">Omschrijving</label>
      <input type="text" id="description" v-model="description" required
             class="inputField">

      <label for="rating" class="label">Beoordeling</label>
      <input type="number" id="rating" v-model.number="rating" required min="0" max="5"
             class="inputField">

      <label for="itemtypes" class="label">Type</label>
      <select name="itemtypes" id="itemtypes" v-model="itemType" required
              class="inputField">
        <option value="" disabled selected>Kies een type...</option>
        <option
            v-for="type in itemTypeList"
            :key="type.value"
            :value="type.value"
        >
          {{ type.name }}
        </option>
      </select>

      <p v-if="errorMessage" class="error mb-4">{{ errorMessage }}</p>
      <button type="button" class="submitButton w-full bg-blue-500 text-white py-2 rounded-lg font-medium" @click="CreateReviewAndClose">Create</button>

      <ErrorBox v-if="errorMessage" :error-text="errorMessage"></ErrorBox>
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
.label{
  display: block;
  font-weight: var(--font-weight-medium);
  margin-bottom: calc(var(--spacing) * 1);
  color: var(--color-gray-700)
}
.inputField {
  width: 100%;
  background-color: #f9fafb;
  border: 1px solid #d1d5db;
  border-radius: 0.5rem;
  padding: 0.5rem 0.75rem;
  margin-bottom: 1rem;
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
</style>