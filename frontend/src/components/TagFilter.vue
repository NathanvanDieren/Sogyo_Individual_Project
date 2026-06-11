<script setup lang="ts">
import { computed, ref, onMounted, onUnmounted } from 'vue'

const props = defineProps<{
  items: Array<{ name: string }>
  modelValue: string[] // Gewijzigd naar array voor multi-select checkboxes
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: string[]): void
}>()

const isOpen = ref(false)
const dropdownRef = ref<HTMLElement | null>(null)

const allNames = computed(() => {
  const namesSet = new Set<string>()
  props.items.forEach(item => {
    if (item.name) {
      namesSet.add(item.name)
    }
  })
  return Array.from(namesSet)
})

const buttonText = computed(() => {
  if (props.modelValue.length === 0) return 'Alles tonen'
  if (props.modelValue.length === 1) return props.modelValue[0]
  return `${props.modelValue.length} geselecteerd`
})

const toggleDropdown = () => {
  isOpen.value = !isOpen.value
}

const toggleTag = (tag: string) => {
  const updatedValue = [...props.modelValue]
  const index = updatedValue.indexOf(tag)

  if (index === -1) {
    updatedValue.push(tag)
  } else {
    updatedValue.splice(index, 1)
  }

  emit('update:modelValue', updatedValue)
}

const clearTags = () => {
  emit('update:modelValue', [])
}

const handleClickOutside = (event: MouseEvent) => {
  if (dropdownRef.value && !dropdownRef.value.contains(event.target as Node)) {
    isOpen.value = false
  }
}

onMounted(() => {
  window.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
  window.removeEventListener('click', handleClickOutside)
})
</script>

<template>
  <div ref="dropdownRef" class="multiselect-dropdown">
    <!-- Dropdown Knop -->
    <button
        class="dropdown-btn"
        :class="{ active: modelValue.length > 0 }"
        @click="toggleDropdown"
        type="button"
    >
      <span>{{ buttonText }}</span>
      <span class="arrow" :class="{ open: isOpen }">▼</span>
    </button>

    <div v-if="isOpen" class="dropdown-content">
      <label class="dropdown-item reset-item">
        <input
            type="checkbox"
            :checked="modelValue.length === 0"
            @change="clearTags"
        />
        <span>Alles tonen</span>
      </label>

      <hr class="dropdown-divider" />

      <label
          v-for="tag in allNames"
          :key="tag"
          class="dropdown-item"
      >
        <input
            type="checkbox"
            :value="tag"
            :checked="modelValue.includes(tag)"
            @change="toggleTag(tag)"
        />
        <span>{{ tag }}</span>
      </label>
    </div>
  </div>
</template>

<style scoped>
.multiselect-dropdown {
  position: relative;
  width: 240px;
  font-family: ui-sans-serif, system-ui, sans-serif;
}

.dropdown-btn {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #cbd5e1;
  background-color: white;
  cursor: pointer;
  border-radius: 4px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 14px;
  color: #1e293b;
  transition: border-color 0.15s ease;
}

.dropdown-btn.active {
  border-color: #3b82f6;
  background-color: #eff6ff;
  color: #1d4ed8;
  font-weight: 500;
}

.arrow {
  font-size: 10px;
  transition: transform 0.2s ease;
  color: #64748b;
}

.arrow.open {
  transform: rotate(180deg);
}

.dropdown-content {
  position: absolute;
  top: 100%;
  left: 0;
  width: 100%;
  background-color: white;
  border: 1px solid #cbd5e1;
  border-radius: 4px;
  box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1);
  margin-top: 4px;
  z-index: 50;
  max-height: 240px;
  overflow-y: auto;
}

.dropdown-item {
  display: flex;
  align-items: center;
  padding: 8px 12px;
  cursor: pointer;
  font-size: 14px;
  color: #334155;
  user-select: none;
}

.dropdown-item:hover {
  background-color: #f8fafc;
}

.dropdown-item input {
  margin-right: 10px;
  cursor: pointer;
}

.dropdown-divider {
  border: 0;
  border-top: 1px solid #e2e8f0;
  margin: 4px 0;
}

.reset-item {
  font-weight: 600;
  color: #1e293b;
}
</style>
