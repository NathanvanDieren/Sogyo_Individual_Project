<script setup lang="ts">
import { ref } from 'vue'
import LogoBar from "../components/LogoBar.vue";
import AddButton from "../components/AddButton.vue";
import NewGroupForm from "../components/NewGroupForm.vue";
import NewReviewForm from "../components/NewReviewForm.vue";

const showGroupModal = ref(false)
const showReviewModal = ref(false)
</script>

<template>
  <LogoBar />

  <AddButton
      @open-group="showGroupModal = true"
      @open-review="showReviewModal = true"
  />

  <NewGroupForm
      v-if="showGroupModal"
      @close="showGroupModal = false"
  />

  <NewReviewForm
      v-if="showReviewModal"
      @close="showReviewModal = false"
  />


</template>

<style scoped>


</style>