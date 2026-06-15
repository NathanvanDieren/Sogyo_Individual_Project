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
          ariaLabel="Open menu"
          class="header-left"
          @click="openMenu"
      >
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round">
          <line x1="3" y1="12" x2="21" y2="12"></line>
          <line x1="3" y1="6" x2="21" y2="6"></line>
          <line x1="3" y1="18" x2="21" y2="18"></line>
        </svg>
      </IconButton>

      <LogoSmall class="header-center" />

      <IconButton
          ariaLabel="Uitloggen"
          class="header-right"
          @click="logout"
      >
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
          <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path>
          <polyline points="16 17 21 12 16 7"></polyline>
          <line x1="21" y1="12" x2="9" y2="12"></line>
        </svg>
      </IconButton>
    </header>
</template>


<style scoped>

.main-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: var(--bg-header);
  color: var(--text-white);
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