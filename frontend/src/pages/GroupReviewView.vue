<script setup lang="ts">

import DisplayReviews from "../components/DisplayReviews.vue";
import LogoBar from "../components/LogoBar.vue";
const props = defineProps<{
  id: string
}>()

</script>

<template>
  <LogoBar />
  <DisplayReviews :id="props.id" />
</template>

<style scoped>
</style>