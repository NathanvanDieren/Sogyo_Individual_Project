<script setup lang="ts">
import router from "../router";

const isOpen = defineModel<boolean>({ default: false })

function displayMyReviews() {
  router.push({ name: 'myreviews' });
}

function displayMyGroups() {
  router.push({ name: 'homepage' });
}
</script>

<template>
    <aside :class="['sidebar', { 'sidebar-closed': !isOpen }]">
      <div class="menu-items">
        <button class="menu-button" @click="displayMyGroups">Mijn Groepen</button>
        <button class="menu-button" @click="displayMyReviews">Mijn Reviews</button>
      </div>
    </aside>
</template>

<style scoped>
.sidebar {
  position: fixed;
  top: 70px;
  left: 0;
  width: 200px;
  height: calc(100vh - 70px);
  background-color: var(--bg-sidebar);
  color: var(--text-white);
  padding: 15px;
  z-index: 100;
  transition: transform 0.3s ease;
}

.sidebar-closed {
  transform: translateX(-100%);
}

.menu-button {
  background: none;
  border: none;
  color: var(--text-muted);
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;

  text-align: left;
  width: 100%;
  padding: 12px 12px;


  transition: all 0.2s ease;
}
.menu-button:hover {
  background-color: var(--bg-tag-hover);
  color: var(--text-white);
  padding-left: 30px;
}


</style>
