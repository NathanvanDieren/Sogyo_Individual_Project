<script setup lang="ts">

</script>

<template>
  <div class="container">
    <form>
      <label for="fname">Username:</label><br>
      <input type="text" id="fname" name="fname"><br>
      <label for="lname">Password:</label><br>
      <input type="text" id="lname" name="lname"><br>
      <input type="submit" value="Submit">
    </form>
  </div>
</template>

<style scoped>
.container {
  height: 200px;
  border: 2px solid;
  border-radius: 0.5em;
  display: flex;
  align-items: center;
  justify-content: center;
}

#submit {

}
</style>