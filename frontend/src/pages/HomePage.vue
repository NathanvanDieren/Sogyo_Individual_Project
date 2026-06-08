<script setup lang="ts">
import { ref } from 'vue'
import LogoBar from "../components/LogoBar.vue";
import AddButton from "../components/AddButton.vue";
import NewGroupForm from "../components/NewGroupForm.vue";
import NewReviewForm from "../components/NewReviewForm.vue";
import DisplayGroups from "../components/DisplayGroups.vue";

const showGroupModal = ref(false)
const showReviewModal = ref(false)

const activeView = ref<'groups' | 'reviews'>('groups')

const displayGroupsRef = ref<InstanceType<typeof DisplayGroups> | null>(null)

const availableGroups = ref<any[]>([])

function handleGroupsLoaded(groups: any[]) {
  availableGroups.value = groups
}
function handleSuccess() {
  showGroupModal.value = false
  displayGroupsRef.value?.GetGroups()
}
</script>

<template>
  <LogoBar />

  <DisplayGroups v-if="activeView === 'groups'" ref="displayGroupsRef" @groups-loaded="handleGroupsLoaded" />

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
      :available-groups="availableGroups"
  />
</template>

<style scoped>

</style>