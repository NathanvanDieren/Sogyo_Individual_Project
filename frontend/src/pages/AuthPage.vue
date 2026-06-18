<script setup lang="ts">
import router from '../router'
import LogoBig from '../components/LogoBig.vue'
import LogoSmall from '../components/LogoSmall.vue'
import {ref} from "vue";
import {apiPost} from "../services/api.ts";
import ErrorBox from "../components/ErrorBox.vue";

const emailInput = ref<string>('')
const passwordInput = ref<string>('')

const errorType = ref<string>('')
const errorText = ref<string>('')

interface LoginProps {
  email: string
  password: string
}

async function LoginAndGoToHome() {
  errorType.value = ''
  errorText.value = ''

  const data: LoginProps = {
    email: emailInput.value,
    password: passwordInput.value,
  }

  try {
    await apiPost<LoginProps, any>('/api/user/login', data)

    router.push('/')
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}
function goToRegister() {
  router.push('/register')
}
</script>

<template>
  <div class="auth-container">
    <div class="auth-info desktop-info">
      <LogoBig class="logo" />
      <h1 class="info-title">Welkom bij Tastebuds!</h1>
      <p class="info-text">
        Het doel van TasteBuds is om mensen op een laagdrempelige en authentieke manier met elkaar in contact te brengen op basis van gedeelde interesses.
        <br>
        TasteBuds biedt een gezamenlijk platform waar gebruikers hun ervaringen met content – zoals boeken, films, video's en blogs – kunnen delen. Het platform fungeert als een plek om enerzijds je eigen favoriete media vast te leggen en anderzijds geïnspireerd te raken door de oprechte interesses van je vrienden.
      </p>
    </div>

    <div class="auth-info mobile-info">
      <LogoSmall class="logo" />
      <h1 class="info-title">Welkom bij TasteBuds!</h1>
    </div>

    <div class="auth-form-container">
      <h2 class="form-title desktop-title">Log hier in</h2>
      <h2 class="form-title mobile-title">Inloggen</h2>

      <form @submit.prevent="LoginAndGoToHome" class="auth-form">
        <label for="fname" class="form-label">Emailadres</label>
        <input type="text" id="fname" name="fname" v-model="emailInput" required
               class="form-input" />

        <label for="lname" class="form-label">Wachtwoord</label>
        <input type="password" id="lname" name="lname" v-model="passwordInput" required
               class="form-input" />

        <ErrorBox :error-type="errorType" :error-text="errorText" @clear-error="errorText = ''" />

        <input type="submit" value="Log In"
               class="submit-button"
               @click="LoginAndGoToHome()">
        <label for="lname" class="register-label">Nog geen account? </label>
        <input type="submit" value="Registreer"
               class="register-button"
               @click="goToRegister()">
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

.form-title {
  font-size: 1.5rem;
  font-weight: bold;
  color: var(--text-primary);
  margin-bottom: 24px;
}

.mobile-title {
  text-align: center;
}

@media (min-width: 768px) {
  .mobile-title {
    display: none;
  }
  
  .desktop-title {
    display: block;
  }
}

@media (max-width: 767px) {
  .desktop-title {
    display: none;
  }
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

.register-label {
  display: block;
  font-weight: 500;
  padding-top: 16px;
  color: var(--text-primary);
  margin-bottom: 4px;
}

.register-button {
  width: 100%;
  background-color: var(--accent-primary);
  color: var(--text-white);
  padding: 10px 0;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  border: none;
  transition: background-color 0.15s ease;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
}

.register-button:hover {
  background-color: var(--accent-primary-hover);
}
</style>
