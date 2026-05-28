<script setup lang="ts">

import IconButton from "./IconButton.vue";
import Logo from "./Logo.vue";
</script>

<template>
  <header class="main-header">
    <IconButton
        ariaLabel="Open account"
        class="header-left"
    >   ☰
       </IconButton>

    <Logo/>

      <IconButton
          ariaLabel="Open account"
          class="header-right"
      >
        👤 </IconButton>

  </header>
</template>

<style scoped>

.main-header {
  display: grid;
  grid-template-columns: 1fr auto 1fr;
  align-items: center;
  height: 70px;
  background-color: #ffffff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 0 20px;
}

.header-left {
  justify-self: start;
}

.header-center {
  justify-self: center;
}

.header-right {
  justify-self: end;
}



</style>