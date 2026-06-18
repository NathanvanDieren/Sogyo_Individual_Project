<script setup lang="ts">
import LogoSmall from "../components/LogoSmall.vue";
import LogoBig from "../components/LogoBig.vue";
import { ref } from 'vue'
import router from '../router'
import { apiPost } from "../services/api.ts";
import ErrorBox from "../components/ErrorBox.vue";

const usernameInput = ref<string>('')
const emailInput = ref<string>('')
const passwordInput = ref<string>('')

const errorType = ref<string>('')
const errorText = ref<string>('')

interface RegisterProps {
  username: string
  email: string
  password: string
}
function goToLogin()
{
  router.push("/")
}
async function RegisterAndGoToAuth() {
  errorType.value = ''
  errorText.value = ''

  const data: RegisterProps = {
    username: usernameInput.value,
    email: emailInput.value,
    password: passwordInput.value,
  }

  try {
    await apiPost<RegisterProps, any>('/api/user/register', data)

    router.push('/login')
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}
</script>

<template>
  <div class="auth-container">
    <div class="auth-info desktop-info">
      <LogoBig class="logo" />
      <h1 class="info-title">Welkom bij TasteBuds!</h1>
      <p class="info-text">
        Vul hier je gegevens in om je te registreren.
      </p>
    </div>

    <div class="auth-info mobile-info">
      <LogoSmall class="logo" />
      <h1 class="info-title">Welkom bij TasteBuds!</h1>
    </div>

    <div class="auth-form-container">
      <div class="navbar">
        <button class="nav-button" @click="goToLogin"><img src="/arrow.png" width="25" height="20"></button>

        <h2 class="form-title">Registreer Account</h2>
      </div>


      <form @submit.prevent="RegisterAndGoToAuth" class="auth-form">

        <label for="username" class="form-label">Gebruikersnaam</label>
        <input type="text" id="username" v-model="usernameInput" required
               class="form-input" />

        <label for="email" class="form-label">Emailadres</label>
        <input type="email" id="email" v-model="emailInput" required
               class="form-input" />

        <label for="password" class="form-label">Wachtwoord</label>
        <input type="password" id="password" v-model="passwordInput" required
               class="form-input" />

        <ErrorBox :error-type="errorType" :error-text="errorText" @clear-error="errorText = ''"/>

        <input type="submit" value="Registreer"
               class="submit-button">

      </form>

    </div>
  </div>
</template>

<style scoped>
.auth-container {
  width: 100%;
  min-height: 100vh;
  background-color: var(--bg-input);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 16px;
}

@media (min-width: 768px) {
  .auth-container {
    flex-direction: row;
    padding: 48px;
    justify-content: center;
    gap: 48px;
  }
}

.auth-info {
  display: none;
}

@media (min-width: 768px) {
  .desktop-info {
    display: block;
    max-width: 400px;
  }
}

.mobile-info {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
}

@media (min-width: 768px) {
  .mobile-info {
    display: none;
  }
}

.info-title {
  font-size: 2rem;
  font-weight: bold;
  color: var(--text-primary);
  margin-bottom: 16px;
}

@media (max-width: 767px) {
  .info-title {
    font-size: 1.5rem;
  }
}

.info-text {
  font-size: 1.125rem;
  color: var(--text-secondary);
}

.auth-form-container {
  width: 100%;
  max-width: 384px;
  background-color: var(--bg-secondary);
  border: 1px solid var(--border-color);
  border-radius: 16px;
  padding: 24px;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
}

@media (min-width: 768px) {
  .auth-form-container {
    max-width: 448px;
    padding: 40px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  }
}

.navbar{
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
}

.nav-button {
  width:fit-content;
  background-color: var(--bg-secondary);
  padding: 2px 2px 0 2px;
  border:none;
  border-radius: 5px;
}
.nav-button:hover {
  background-color: lightgray
}

.form-title {
  font-weight: bold;
  color: var(--text-primary);
  font-size: 24px;
  margin: 10px 0;
}

.auth-form {
  width: 100%;
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

.submit-button {
  width: 100%;
  background-color: var(--accent-primary);
  color: var(--text-white);
  padding: 10px 0;
  margin: 10px 0;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  border: none;
  transition: background-color 0.15s ease;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
}

.submit-button:hover {
  background-color: var(--accent-primary-hover);
}
</style>
