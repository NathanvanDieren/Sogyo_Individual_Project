<script setup lang="ts">
import ReviewsGroup from "../components/ReviewsGroup.vue";

const props = defineProps<{
  id: string
  name: string
  members: string[]
}>()
</script>

<template>
  <ReviewsGroup :id="props.id" :name="props.name" :members="props.name"/>
</template>

<style scoped>
</style>
