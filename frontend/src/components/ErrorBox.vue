<script setup lang="ts">

interface Props {
  errorType?: string
  errorText: string
}
withDefaults(defineProps<Props>(), {
  errorType: 'Fout'
})
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