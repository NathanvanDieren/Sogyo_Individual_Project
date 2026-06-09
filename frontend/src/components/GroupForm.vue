<script setup lang="ts">
import ErrorBox from "./ErrorBox.vue";
import {ref, onMounted, watch } from 'vue'
import { apiPost } from "../services/api.ts";

interface GroupEditData {
  id: string
  name: string
  members: string[]
}

const props = defineProps<{
  groupToEdit?: GroupEditData | null
}>()

const emit = defineEmits(['close', 'success'])

const nameInput = ref<string>('')
const emailList = ref<string[]>([])
const newEmail = ref<string>('')
const errorMessage = ref<string>('')

const errorType = ref<string>('')
const errorText = ref<string>('')

// Regex voor e-mailvalidatie
const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

function initializeForm() {
  errorType.value = ''
  errorText.value = ''
  errorMessage.value = ''

  if (props.groupToEdit) {
    nameInput.value = props.groupToEdit.name
    emailList.value = props.groupToEdit.members
        ? props.groupToEdit.members.map((m: any) => m.email)
        : []
  } else {
    nameInput.value = ''
    emailList.value = []
  }
}

onMounted(initializeForm)
watch(() => props.groupToEdit, initializeForm)

const addEmail = (): void => {
  const trimmedEmail: string = newEmail.value.trim()
  errorMessage.value = ''

  if (!trimmedEmail) {
    errorMessage.value = 'Het veld mag niet leeg zijn.'
    return
  }

  if (!emailRegex.test(trimmedEmail)) {
    errorMessage.value = 'Voer een geldig e-mailadres in.'
    return
  }

  if (emailList.value.includes(trimmedEmail)) {
    errorMessage.value = 'Dit e-mailadres is al toegevoegd.'
    return
  }

  emailList.value.push(trimmedEmail)
  newEmail.value = ''
}

const removeEmail = (index: number): void => {
  emailList.value.splice(index, 1)
}

async function saveGroupAndClose() {
  errorType.value = ''
  errorText.value = ''

  if (!nameInput.value.trim()) {
    errorType.value = 'Validatie Fout'
    errorText.value = 'Groepsnaam is verplicht.'
    return
  }

  interface SaveGroupProps {
    name: string,
    emails: string[]
  }

  const data: SaveGroupProps = {
    name: nameInput.value,
    emails: emailList.value
  }

  try {
    if (props.groupToEdit) {
      await apiPost<SaveGroupProps, any>(`/api/group/edit/${props.groupToEdit.id}`, data)
    } else {
      await apiPost<SaveGroupProps, any>('/api/group/create', data)
    }

    emit('success')
    emit('close')
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}
</script>

<template>
  <div class="modal-overlay" @click="emit('close')">
    <form class="modal-content" @click.stop @submit.prevent="saveGroupAndClose">

      <h2 class="font-bold text-2xl mb-4">
        {{ props.groupToEdit ? 'Groep bewerken' : 'Creëer nieuwe groep' }}
      </h2>

      <label for="fname" class="block font-medium text-gray-700 mb-1">Naam van de groep:</label>
      <input
          type="text"
          id="fname"
          name="fname"
          v-model="nameInput"
          required
          class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-4 focus:outline-none focus:border-blue-500 bg-gray-50"
      >

      <div class="email-manager mb-4">
        <h3>E-mailadressen van groepsleden</h3>

        <div class="input-group">
          <input
              v-model="newEmail"
              type="email"
              placeholder="vriend@voorbeeld.nl"
              @keyup.enter.prevent="addEmail"
          />
          <button @click="addEmail" type="button">Toevoegen</button>
        </div>
        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>

        <ul class="email-list">
          <li v-for="(email, index) in emailList" :key="index">
            {{ email }}
            <button @click="removeEmail(index)" class="remove-btn" type="button">×</button>
          </li>
        </ul>

        <p v-if="emailList.length > 0" class="meta">
          Totaal aantal adressen: {{ emailList.length }}
        </p>
      </div>

      <ErrorBox v-if="errorText" :error-type="errorType" :error-text="errorText"></ErrorBox>

      <div class="flex gap-2 mt-4">
        <button type="button" class="cancelButton" @click="emit('close')">Annuleren</button>
        <button type="submit" class="submitButton">
          {{ props.groupToEdit ? 'Opslaan' : 'Creëren' }}
        </button>
      </div>
    </form>
  </div>
</template>

<style scoped>
.submitButton {
  width: clamp(100px, 15vw, 200px);
  height: clamp(40px, 6vh, 70px);
  font-size: clamp(0.875rem, 1.2vw, 1.25rem);

  border-radius: 10px;
  background-color: #007bff;
  color: white;
  cursor: pointer;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background-color 0.2s, transform 0.2s;
}

.submitButton:hover {
  background-color: #0056b3;
}

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

.email-manager {
  max-width: 400px;
  font-family: sans-serif;
}
.input-group {
  display: flex;
  gap: 10px;
}
input {
  flex: 1;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
button {
  padding: 8px 12px;
  background-color: #42b883;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.email-list {
  list-style: none;
  padding: 0;
  margin-top: 15px;
}
.email-list li {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 6px 10px;
  background-color: #f3f3f3;
  margin-bottom: 5px;
  border-radius: 4px;
}
.remove-btn {
  background-color: #ff4d4d;
  padding: 2px 6px;
  font-size: 12px;
}

.error {
  color: red;
  font-size: 14px;
  margin-top: 5px;
}
.meta {
  font-size: 12px;
  color: #666;
}
.flex {
  display: flex;
}
.gap-2 {
  gap: 8px;
}
.mt-4 {
  margin-top: 16px;
}
.cancelButton {
  padding: 8px 16px;
  background-color: #64748b;
  color: white;
  border: none;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 500;
}
.cancelButton:hover {
  background-color: #475569;
}
</style>