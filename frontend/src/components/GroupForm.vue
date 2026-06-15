<script setup lang="ts">
import ErrorBox from "./ErrorBox.vue";
import {ref, onMounted, watch } from 'vue'
import { apiPost } from "../services/api.ts";

interface GroupEditData {
  id: string
  name: string
  emails: string[]
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
    emailList.value = props.groupToEdit.emails

  } else {
    nameInput.value = ''
    emailList.value = []
  }
}

onMounted(initializeForm)
watch(
    () => props.groupToEdit,
    () => {
      initializeForm()
    },
    { immediate: true }
)

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

      <h2 class="modal-title">
        {{ props.groupToEdit ? 'Groep bewerken' : 'Creëer nieuwe groep' }}
      </h2>

      <label for="fname" class="form-label">Naam van de groep:</label>
      <input
          type="text"
          id="fname"
          name="fname"
          v-model="nameInput"
          required
          class="form-input"
      >

      <div class="email-manager">
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

      <ErrorBox v-if="errorText" :error-type="errorType" :error-text="errorText" @clear-error="errorText = ''"></ErrorBox>

      <div class="form-actions">
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
  background-color: var(--accent-primary);
  color: var(--text-white);
  cursor: pointer;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background-color 0.2s, transform 0.2s;
}

.submitButton:hover {
  background-color: var(--accent-primary-hover);
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
  background: var(--bg-secondary);
  padding: 30px;
  border-radius: 8px;
  min-width: 300px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.2);
}

.modal-title {
  font-weight: bold;
  font-size: 1.5rem;
  margin-bottom: 16px;
}

.form-label {
  display: block;
  font-weight: 500;
  color: var(--text-primary);
  margin-bottom: 4px;
}

.form-input {
  width: 100%;
  border: 1px solid var(--border-color);
  border-radius: 8px;
  padding: 8px 12px;
  margin-bottom: 16px;
  background-color: var(--bg-input);
  box-sizing: border-box;
}

.form-input:focus {
  outline: none;
  border-color: var(--accent-primary);
}

.email-manager {
  max-width: 400px;
  font-family: sans-serif;
  margin-bottom: 16px;
}

.input-group {
  display: flex;
  gap: 10px;
}

input {
  flex: 1;
  padding: 8px;
  border: 1px solid var(--border-color);
  border-radius: 4px;
}

button {
  padding: 8px 12px;
  background-color: var(--accent-success);
  color: var(--text-white);
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
  background-color: var(--bg-input);
  margin-bottom: 5px;
  border-radius: 4px;
}

.remove-btn {
  background-color: var(--accent-danger);
  padding: 2px 6px;
  font-size: 12px;
}

.error {
  color: var(--text-error);
  font-size: 14px;
  margin-top: 5px;
}

.meta {
  font-size: 12px;
  color: var(--text-secondary);
}

.form-actions {
  display: flex;
  gap: 8px;
  margin-top: 16px;
}

.cancelButton {
  padding: 8px 16px;
  background-color: var(--text-secondary);
  color: var(--text-white);
  border: none;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 500;
}

.cancelButton:hover {
  background-color: var(--text-secondary-hover);
}
</style>
