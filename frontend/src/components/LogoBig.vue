<script setup lang="ts">

</script>

<template>
  <div class="header-center">
    <img class="logo" src="/TasteBuds-logo.png" alt="TasteBuds logo">
  </div>
</template>

<style scoped>
.logo {
  height: 200px;         /* De hoogte die je wilt */
  width: min-content;    /* MAGIE: Dwingt de container om zo smal te worden als het logo zelf */
  display: block;        /* Nodig om de breedte-berekening goed te laten werken */
  object-fit: contain;

  border-radius: 16px;
  background-color: white; /* Geeft de loze ruimte een nette achtergrond */
}
</style>