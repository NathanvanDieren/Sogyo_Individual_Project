<script setup lang="ts">
import { ref } from 'vue'
import LogoBar from "../components/LogoBar.vue";
import AddButton from "../components/AddButton.vue";
import NewGroupForm from "../components/NewGroupForm.vue";
import NewReviewForm from "../components/NewReviewForm.vue";
import DisplayGroups from "../components/DisplayGroups.vue";
import DisplayReviews from "../components/DisplayReviews.vue";

const showGroupModal = ref(false)
const showReviewModal = ref(false)

const activeView = ref<'groups' | 'reviews'>('groups')

const displayGroupsRef = ref<InstanceType<typeof DisplayGroups> | null>(null)

function handleSuccess() {
  showGroupModal.value = false
  displayGroupsRef.value?.GetGroups()
}
</script>

<template>
  <LogoBar />

  <div class="toggle-container">
    <button
        :class="{ active: activeView === 'groups' }"
        @click="activeView = 'groups'"
    >
      Groepen
    </button>
    <button
        :class="{ active: activeView === 'reviews' }"
        @click="activeView = 'reviews'"
    >
      Reviews
    </button>
  </div>

  <DisplayGroups v-if="activeView === 'groups'" ref="displayGroupsRef" />
  <DisplayReviews v-slot v-if="activeView === 'reviews'" />

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
.toggle-container {
  display: flex;
  justify-content: center;
  gap: 10px;
  margin: 20px 0;
}

.toggle-container button {
  padding: 10px 20px;
  font-size: 1rem;
  border: 1px solid #ccc;
  background-color: #fff;
  cursor: pointer;
  border-radius: 5px;
  transition: all 0.2s ease;
}


.toggle-container button.active {
  background-color: #42b883;
  border-color: #42b883;
}
</style>