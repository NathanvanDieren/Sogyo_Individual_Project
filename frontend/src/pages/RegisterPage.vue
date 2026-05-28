<script setup lang="ts">
import LogoSmall from "../components/LogoSmall.vue";
import LogoBig from "../components/LogoBig.vue";
</script>

<template>
  <div class="w-full min-h-screen bg-gray-50 flex flex-col items-center justify-center p-4
              md:flex-row md:p-12 md:justify-around">

    <div class="hidden md:block max-w-md">
      <LogoBig class="block mx-auto mb-6 w-32" />
      <h1 class="text-4xl font-bold text-gray-900 mb-4">Welkom bij TasteBuds!</h1>
      <p class="text-gray-600 text-lg">
        Vul hier je gegevens in om je te registreren.
      </p>
    </div>

    <div class="block md:hidden">
      <LogoSmall class="block mx-auto mb-6 w-32" />
      <h1 class="text-2xl font-bold text-gray-900 mb-4">Welkom bij TasteBuds!</h1>
    </div>

    <div class="w-full max-w-sm bg-white border border-gray-200 rounded-2xl p-6 shadow-sm
                md:max-w-md md:p-10 md:shadow-lg">

      <h2 class="text-2xl font-bold text-gray-800 mb-6 md:hidden text-center">Register Account</h2>
      <h2 class="text-2xl font-bold text-gray-800 mb-6 hidden md:block">Register Account</h2>

      <form @submit.prevent="$router.push('/')" class="w-full">
        <label for="fname" class="block font-medium text-gray-700 mb-1">Username</label>
        <input type="text" id="fname" name="fname"
               class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-4 focus:outline-none focus:border-blue-500 bg-gray-50">

        <label for="lname" class="block font-medium text-gray-700 mb-1">Email-address</label>
        <input type="text" id="fname" name="fname"
               class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-6 focus:outline-none focus:border-blue-500 bg-gray-50">

        <label for="lname" class="block font-medium text-gray-700 mb-1">Password</label>
        <input type="password" id="lname" name="lname"
               class="w-full border border-gray-300 rounded-lg px-3 py-2 mb-6 focus:outline-none focus:border-blue-500 bg-gray-50">

        <input type="submit" value="Register"
               class="w-full bg-blue-600 text-white py-2.5 rounded-lg font-semibold cursor-pointer hover:bg-blue-700 transition shadow-sm">
      </form>

    </div>
  </div>
</template>
