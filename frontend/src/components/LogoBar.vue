<script setup lang="ts">

import IconButton from "./IconButton.vue";
import LogoSmall from "./LogoSmall.vue";
import {apiPost} from "../services/api.ts";
import router from "../router";

const emit = defineEmits(['open-menu'])
function openMenu() {
    emit("open-menu");
}

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
        @click = openMenu
    >   ☰
       </IconButton>

    <LogoSmall class="header-center" />

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
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #007bff;
  color: #ffffff;
  height: 70px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 10px 10px;
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