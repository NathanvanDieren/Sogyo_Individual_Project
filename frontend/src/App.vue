<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRoute } from 'vue-router'
import LogoBar from './components/LogoBar.vue'
import SidebarMenu from './components/SidebarMenu.vue'

const route = useRoute()
const showMenuModal = ref(false)

const showLayout = computed(() => {
  return route.meta.requiresAuth === true
})
</script>

<template>
  <template v-if="showLayout">
    <LogoBar @open-menu="showMenuModal = true" />

    <div
      v-if="showMenuModal"
      class="sidebar-overlay"
      @click="showMenuModal = false"
      style="z-index: 99;"
    ></div>

    <SidebarMenu v-model="showMenuModal" />

    <main class="main-content">
      <RouterView />
    </main>
  </template>

  <template v-else>
    <RouterView />
  </template>
</template>

<style scoped>
.sidebar-overlay {
  position: fixed;
  top: 70px;
  left: 0;
  width: 100vw;
  height: calc(100vh - 70px);
  background-color: rgba(0, 0, 0, 0.4);
  z-index: 98;
}

.main-content {
  padding: 20px;
  min-height: calc(100vh - 70px);
  background-color: #f8fafc;
}
</style>
