<script setup lang="ts">
import ReviewsGroup from "../components/ReviewsGroup.vue";

const props = defineProps<{
  id: string
}>()
</script>

<template>
  <ReviewsGroup :id="props.id" />
</template>

<style scoped>
</style>
