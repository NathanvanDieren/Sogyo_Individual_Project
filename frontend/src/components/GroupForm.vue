<script setup lang="ts">
import ErrorBox from "./ErrorBox.vue";
import { ref, onMounted, watch } from 'vue'
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
          <button @click="addEmail" type="button" class="add-btn">Toevoegen</button>
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
/* Modal Basis */
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
  padding: 24px;
  border-radius: 8px;
  width: 100%;
  max-width: 420px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.2);
  box-sizing: border-box;
}

.modal-title {
  font-weight: bold;
  font-size: 1.35rem;
  margin-top: 0;
  margin-bottom: 16px;
}

.form-label {
  display: block;
  font-weight: 500;
  color: var(--text-primary);
  margin-bottom: 6px;
  font-size: 14px;
}

.form-input {
  width: 100%;
  border: 1px solid var(--border-color);
  border-radius: 6px;
  padding: 6px 10px;
  margin-bottom: 16px;
  background-color: var(--bg-input);
  box-sizing: border-box;
  font-size: 14px;
}

.form-input:focus {
  outline: none;
  border-color: var(--accent-primary);
}

.email-manager {
  margin-bottom: 20px;
}

.email-manager h3 {
  font-size: 14px;
  margin-top: 0;
  margin-bottom: 8px;
  font-weight: 500;
}

.input-group {
  display: flex;
  gap: 8px;
}

.input-group input {
  flex: 1;
  padding: 6px 10px;
  border: 1px solid var(--border-color);
  border-radius: 6px;
  font-size: 14px;
  background-color: var(--bg-input);
}

.add-btn {
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
  margin: 10px 0 0 0;
  max-height: 150px;
  overflow-y: auto;
}

.email-list li {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 4px 8px;
  background-color: var(--bg-input);
  margin-bottom: 4px;
  border-radius: 4px;
  font-size: 13px;
}

.remove-btn {
  background-color: var(--accent-danger);
  color: var(--text-white);
  border: none;
  border-radius: 4px;
  padding: 2px 6px;
  font-size: 12px;
  cursor: pointer;
}

.error {
  color: var(--text-error);
  font-size: 13px;
  margin: 4px 0 0 0;
}

.meta {
  font-size: 12px;
  color: var(--text-secondary);
  margin: 6px 0 0 0;
}

.form-actions {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-top: 24px;
}

.cancelButton, .submitButton {
  border: none;
  border-radius: 6px;
  font-weight: 500;
  font-size: 13px;
  white-space: nowrap;
  cursor: pointer;
  box-sizing: border-box;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.cancelButton {
  padding: 6px 16px;
  background-color: var(--text-secondary);
  color: var(--text-white);
}

.cancelButton:hover {
  background-color: var(--text-secondary-hover);
}

.submitButton {
  flex: 1;
  padding: 6px 16px;
  background-color: var(--accent-primary);
  color: var(--text-white);
}

.submitButton:hover {
  background-color: var(--accent-primary-hover);
}
</style>