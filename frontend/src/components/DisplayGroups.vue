<script setup lang="ts">
import {apiGet} from "../services/api.ts";
import ErrorBox from "./ErrorBox.vue";
import {GroupListDto} from "../Dtos/GroupDtos.ts"
import { ref } from 'vue'


const errorType = ref<string>('')
const errorText = ref<string>('')

const groupList = ref<GroupListDto | null>(null)
defineExpose({
  GetGroups
})

GetGroups().then(() => {
  if (groupList.value?.groups) {
    groupList.value.groups.forEach(group => {
      console.log(group.name)
    })
  }
})

async function GetGroups() {
  try {
    groupList.value = await apiGet<GroupListDto>('/api/group/getgroups')
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}
</script>

<template>
  <ErrorBox v-if="errorText" :error-text="errorText" :error-type="errorType" />

  <div v-else-if="groupList && groupList.groups.length > 0">
    <p>Totaal aantal groepen: {{ groupList.totalCount }}</p>

    <ul>
      <li v-for="group in groupList.groups" :key="group.id" class="group-item">
        <h3>{{ group.name }}</h3>

        <small>Leden ({{ group.members.length }}):</small>
        <ul>
          <li v-for="member in group.members" :key="member.id">
            {{ member.name }}
          </li>
        </ul>
      </li>
    </ul>
  </div>

  <p v-else>Groepen laden...</p>
</template>


<style scoped>

</style>