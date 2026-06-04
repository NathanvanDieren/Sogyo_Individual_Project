<script setup lang="ts">
import { ref } from 'vue'
import LogoBar from "../components/LogoBar.vue";
import AddButton from "../components/AddButton.vue";
import NewGroupForm from "../components/NewGroupForm.vue";
import NewReviewForm from "../components/NewReviewForm.vue";
import DisplayGroups from "../components/DisplayGroups.vue";

const showGroupModal = ref(false)
const showReviewModal = ref(false)
const showDisplayGroups = ref(true)

const displayGroupsRef = ref<InstanceType<typeof DisplayGroups> | null>(null)

function handleSuccess() {
  // Sluit de modal netjes af
  showReviewModal.value = false

  displayGroupsRef.value?.GetGroups()
}
</script>

<template>
  <LogoBar />

  <DisplayGroups v-if="showDisplayGroups" ref="displayGroupsRef" />

  <AddButton
      @open-group="showGroupModal = true"
      @open-review="showReviewModal = true"
  />

  <NewGroupForm
      v-if="showGroupModal"
      @close="showGroupModal = false"
      @success="handleSuccess"
  />

  <NewReviewForm
      v-if="showReviewModal"
      @close="showReviewModal = false"
  />
</template>

<style scoped>
</style>
