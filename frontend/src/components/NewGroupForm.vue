<script setup lang="ts">
import ErrorBox from "./ErrorBox.vue";
import {Ref, ref} from 'vue'
import {apiPost} from "../services/api.ts";

const newEmail: Ref<string> = ref('')
const errorMessage: Ref<string> = ref('')
const emailList: Ref<string[]> = ref([])

const nameInput = ref<string>('')
const errorType = ref<string>('')
const errorText = ref<string>('')

const emit = defineEmits(['close', 'success'])

// Regex voor e-mailvalidatie
const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

// Logica voor toevoegen
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

async function CreateGroupAndClose() {
  errorType.value = ''
  errorText.value = ''

  interface CreateGroupProps {
    name: String,
    emails: String[]
  }
  const data: CreateGroupProps = {
    name: nameInput.value,
    emails: emailList.value

  }

  try {
    await apiPost<CreateGroupProps, any>('/api/group/create', data)

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
    <form class="modal-content" @click.stop>
      <h2 class="font-bold text-2xl">Create new group</h2>
      <label for="fname"  class="block font-medium text-gray-700 mb-1">Name</label>
      <input type="text" id="fname" name="fname" v-model="nameInput" required
             class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-4 focus:outline-none focus:border-blue-500 bg-gray-50">

      <div class="email-manager">
        <h3>E-mailadressen toevoegen</h3>

        <div class="input-group">
          <input
              v-model="newEmail"
              type="email"
              placeholder="vriend@voorbeeld.nl"
              @keyup.enter="addEmail"
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
      <button type="button" class = "submitButton" @click="CreateGroupAndClose">Create</button>
      <ErrorBox error-text=""></ErrorBox>
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
.error {
  color: red;
  font-size: 14px;
  margin-top: 5px;
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
.meta {
  font-size: 12px;
  color: #666;
}
</style>