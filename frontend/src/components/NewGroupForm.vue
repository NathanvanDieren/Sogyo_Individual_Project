<script setup lang="ts">
const emit = defineEmits(['close'])
</script>

<template>
  <div class="modal-overlay" @click="emit('close')">

    <form class="modal-content" @click.stop>
      <h2>Create a New Group</h2>

      <button type="button" @click="emit('close')">Close</button>
    </form>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 10000;
}

.modal-content {
  background: white;
  padding: 30px;
  border-radius: 8px;
  min-width: 300px;
  box-shadow: 0 4px 15px rgba(0,0,0,0.2);
}
</style>