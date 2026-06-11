<script setup lang="ts">
import { watch } from 'vue' // Zorg dat watch is geïmporteerd

interface props {
  errorType?: string
  errorText: string
}

const props = withDefaults(defineProps<props>(), {
  errorType: 'Fout'
})

const emit = defineEmits(['clear-error'])

let timer: ReturnType<typeof setTimeout> | null = null

watch(() => props.errorText, (newVal) => {
  if (timer) clearTimeout(timer)

  if (newVal) {
    timer = setTimeout(() => {
      emit('clear-error')
    }, 5000)
  }
}, { immediate: true })
</script>

<template>
  <div v-if="errorText" class="error-box">
    <strong>⚠️ {{ errorType }}</strong>
    <p>{{ errorText }}</p>
  </div>
</template>

<style scoped>
.error-box {
  background-color: #fde8e8;
  border: 1px solid #f8b4b4;
  color: #9b1c1c;
  padding: 12px;
  border-radius: 6px;
  margin-bottom: 16px;
}
.error-box strong {
  display: block;
  margin-bottom: 4px;
}
.error-box p {
  margin: 0;
  font-size: 0.9em;
}
</style>