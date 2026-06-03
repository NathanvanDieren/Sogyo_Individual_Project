<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'

const isOpen = ref(false)
const menuRef = ref<HTMLElement | null>(null)

const toggleMenu = () => {
  isOpen.value = !isOpen.value
}

const closeMenu = () => {
  isOpen.value = false
}

const emit = defineEmits(['open-group', 'open-review'])

const handleNewGroup = () => {
  closeMenu()
  emit('open-group') // Stuur seintje naar de homepage
}

const handleNewReview = () => {
  closeMenu()
  emit('open-review') // Stuur seintje naar de homepage
}

const handleClickOutside = (event: MouseEvent) => {
  if (menuRef.value && !menuRef.value.contains(event.target as Node)) {
    closeMenu()
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

<template>
  <div class="menu-container" ref="menuRef">
    <button
        @click="toggleMenu"
        :aria-expanded="isOpen"
        aria-controls="submenu"
        class="floating-button"
    >
      <span class="plus-icon" :class="{ 'rotate': isOpen }">+</span>
    </button>

    <ul v-show="isOpen" id="submenu" class="submenu">
      <li><a @click="handleNewGroup">New Group</a></li>
      <li><a @click="handleNewReview">New Review</a></li>
    </ul>
  </div>
</template>

<style scoped>
.menu-container {
  position: fixed;
  bottom: 20px;
  right: 20px;
  z-index: 9999;
}


.floating-button {
  width: 56px;
  height: 56px;
  border-radius: 50%;
  background-color: #007bff;
  color: white;
  border: none;
  cursor: pointer;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background-color 0.2s, transform 0.2s;
}

.floating-button:hover {
  background-color: #0056b3;
}
.plus-icon {
  font-size: 28px;
  font-weight: 300;
  line-height: 1;
  display: inline-block;
  transition: transform 0.3s ease;
  transform-origin: center;
  margin-top: -2px;
}

.plus-icon.rotate {
  transform: rotate(90deg);
}

.submenu {
  position: absolute;
  bottom: 110%;
  right: 0;
  background-color: white;
  border: 1px solid #ddd;
  list-style: none;
  padding: 8px 0;
  margin: 0;
  min-width: 160px;
  box-shadow: 0 -4px 12px rgba(0, 0, 0, 0.15);
  border-radius: 8px;
}

.submenu li {
  margin: 0;
}

.submenu a {
  text-decoration: none;
  color: #333;
  display: block;
  padding: 10px 20px;
  cursor: pointer;
  font-family: sans-serif;
  font-size: 14px;
}

.submenu a:hover {
  background-color: #f8f9fa;
  color: #007bff;
}
</style>