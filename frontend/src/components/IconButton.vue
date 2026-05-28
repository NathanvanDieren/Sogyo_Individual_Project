<script setup lang="ts">

defineProps<{
  ariaLabel: string;
}>();

defineEmits<{
  (e: 'click'): void;
}>();
</script>

<template>
  <button
      class="icon-btn"
      :aria-label="ariaLabel"
      @click="$emit('click')"
  >
    <slot />
  </button>
</template>

<style scoped>
.icon-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;


  width: 44px;
  height: 44px;
  border-radius: 50%;

  -webkit-tap-highlight-color: transparent;
}

.icon-btn:active {
  background-color: rgba(0, 0, 0, 0.1);
}
</style>