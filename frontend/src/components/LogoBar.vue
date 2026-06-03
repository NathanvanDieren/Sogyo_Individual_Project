<script setup lang="ts">

import IconButton from "./IconButton.vue";
import LogoSmall from "./LogoSmall.vue";
import {apiPost} from "../services/api.ts";
import router from "../router";

async function logout() {
  try {
    await apiPost<object, any>('/api/user/logout', {})

    router.push('/login')
  } catch (err: any) {
    console.error("Uitloggen mislukt:", err)
  }
}
</script>

<template>
  <header class="main-header">
    <IconButton
        ariaLabel="Open account"
        class="header-left"
    >   ☰
       </IconButton>

    <LogoSmall/>

      <IconButton
          ariaLabel="Open account"
          class="header-right"
          @click = logout
      >
        ➜] </IconButton>

  </header>
</template>

<style scoped>

.main-header {
  display: grid;
  grid-template-columns: 1fr auto 1fr;
  align-items: center;
  height: 70px;
  background-color: #007bff;
  color: #ffffff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 0 20px;
}

.header-left {
  justify-self: start;
}

.header-center {
  justify-self: center;
}

.header-right {
  justify-self: end;
}



</style>