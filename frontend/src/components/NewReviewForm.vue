<script setup lang="ts">
import ErrorBox from "./ErrorBox.vue";
import {Ref, ref} from 'vue'
import {apiPost} from "../services/api.ts";

const titleInput = ref<string>('')
const description = ref<string>('')
const rating = ref<number>(0)
const itemType = ref<string>('')

const errorMessage: Ref<string> = ref('')

const emit = defineEmits(['close', 'success'])


async function CreateReviewAndClose() {

  interface CreateReviewResponse {
    titleInput: string,
    description: string,
    rating: number,
    itemType: string
  }
  const data: CreateReviewResponse = {
    titleInput: titleInput.value,
    description: description.value,
    rating: rating.value,
    itemType: itemType.value
  }

  try {
    await apiPost<CreateReviewResponse, any>('/api/group/create', data)

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
      <h2 class="font-bold text-2xl">Create new group</h2>
      <label for="fname"  class="block font-medium text-gray-700 mb-1">Name</label>
      <input type="text" id="fname" name="fname" v-model="titleInput" required
             class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-4 focus:outline-none focus:border-blue-500 bg-gray-50">

      <label for="fname"  class="block font-medium text-gray-700 mb-1">Description</label>
      <input type="text" id="fname" name="fname" v-model="description" required
             class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-4 focus:outline-none focus:border-blue-500 bg-gray-50">

      <label for="fname"  class="block font-medium text-gray-700 mb-1">Rating</label>
      <input type="text" id="fname" name="fname" v-model="rating" required
             class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-4 focus:outline-none focus:border-blue-500 bg-gray-50">

      <label for="fname"  class="block font-medium text-gray-700 mb-1">itemType</label>
      <input type="text" id="fname" name="fname" v-model="itemType" required
             class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-4 focus:outline-none focus:border-blue-500 bg-gray-50">

      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      <button type="button" class = "submitButton" @click="CreateReviewAndClose">Create</button>
      <ErrorBox error-text=""></ErrorBox>
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
  min-width: 300px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.2);
}
</style>