<script setup lang="ts">
import {apiDelete, apiGet} from "../services/api.ts";
import ErrorBox from "./ErrorBox.vue";
import {GroupDto, GroupListDto, GroupMemberDto} from "../dtos/GroupDtos.ts"
import { ref } from 'vue'
import router from "../router";
import GroupForm from "./GroupForm.vue";

const showGroupModal = ref(false);

const selectedGroup = ref<any>(null);

const errorType = ref<string>('')
const errorText = ref<string>('')

const emit = defineEmits<{
  (e: 'groups-loaded', groups: any[]): void
}>()

const groupList = ref<GroupListDto | null>(null)
defineExpose({ GetGroups })

GetGroups().then(() => {
  if (groupList.value?.groups) {
    emit('groups-loaded', groupList.value.groups)
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

function editGroup(group: GroupDto) {
  const existingEmails: string[] = group.members
      ? group.members.map((member: GroupMemberDto) => member.email)
      : [];

  selectedGroup.value = {
    id: group.id,
    name: group.name,
    emails: existingEmails
  };

  showGroupModal.value = true;
}

async function handleFormSuccess() {
  await GetGroups();
  if (groupList.value?.groups) {
    emit('groups-loaded', groupList.value.groups)
  }
}
function openCreateModal() {
  showGroupModal.value = true
  selectedGroup.value = null
}

function goToGroup(groupId: string) {
  router.push({ name: 'GroupReviews', params: { id: groupId } })
}

async function deleteGroup(groupId: string) {
  const bevestigd = confirm("Weet je zeker dat je deze groep wilt verwijderen? Dit kan niet ongedaan worden gemaakt.");
  if (!bevestigd) return;

  try {
    await apiDelete(`/api/group/delete/${groupId}`)
    await GetGroups()
    if (groupList.value?.groups) {
      emit('groups-loaded', groupList.value.groups)
    }
  } catch (err: any) {
    errorType.value = err.type || 'Fout'
    errorText.value = err.message || 'Er is een onbekende fout opgetreden.'
  }
}
</script>

<template>
  <ErrorBox v-if="errorText" :error-text="errorText" :error-type="errorType" />

  <GroupForm
      v-if="showGroupModal"
      :group-to-edit="selectedGroup"
      @close="showGroupModal = false"
      @success="handleFormSuccess"
  />

  <div v-if="groupList" class="groups-container">
    <button @click="openCreateModal" class="create-main-btn">+ Nieuwe Groep</button>

    <p v-if="groupList.groups.length > 0" class="total-count">Groepen: {{ groupList.totalCount }}</p>
    <ul v-if="groupList.groups.length > 0" class="groups-list">
      <li
          v-for="group in groupList.groups"
          :key="group.id"
          class="group-item"
          @click="goToGroup(group.id)"
          role="button"
          tabindex="0"
      >
        <div class="group-header">
          <h3>{{ group.name }}</h3>
          <div class="editbuttoncontainer">
            <button @click.stop="editGroup(group)" class="editbutton">
              Edit
            </button>
            <button @click.stop="deleteGroup(group.id)" class="deletebutton">
              Delete
            </button>
          </div>
        </div>

        <small class="members-title">👤: {{ group.members.length }}</small>
      </li>
    </ul>

    <p v-else-if="groupList.groups.length === 0">Nog geen groepen aangemaakt</p>
  </div>
  <p v-else>Groepen laden...</p>
</template>


<style scoped>
.groups-container {
  max-width: 500px;
  margin: 40px auto;
  padding: 20px;
  font-family: sans-serif;
}

.total-count {
  text-align: left;
  font-weight: bold;
  color: #555;
  margin-bottom: 20px;
}


.groups-list, .members-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.editbutton {
  font-size: 0.8rem;
  color: black;
  background-color: mediumseagreen;
  opacity: 0.7;
  padding: 4px 8px;
  margin: 4px;
  border-radius: 6px;
  font-weight: 500;
  white-space: nowrap;
}

.deletebutton {
  font-size: 0.8rem;
  color: white;
  background-color: red;
  opacity: 0.7;
  padding: 4px 8px;
  margin: 4px;
  border-radius: 6px;
  font-weight: 500;
  white-space: nowrap;
}

.group-item {
  background-color: #ffffff;
  border: 2px solid #e0e0e0;
  border-radius: 12px;
  padding: 20px;
  cursor: pointer;
  transition: all 0.2s ease-in-out;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
}

.group-item:hover {
  border-color: #3b82f6;
  transform: translateY(-2px);
  box-shadow: 0 6px 12px rgba(59, 130, 246, 0.15);
}

.group-item:active {
  transform: translateY(0);
  background-color: #f8fafc;
}

.group-item h3 {
  margin-top: 0;
  margin-bottom: 8px;
  color: #1e293b;
}

.members-title {
  display: block;
  color: #64748b;
  font-weight: 600;
  margin-bottom: 6px;
}

.members-list li {
  font-size: 0.9rem;
  color: #334155;
  padding: 4px 0;
  border-bottom: 1px dashed #f1f5f9;
}

.members-list li:last-child {
  border-bottom: none;
}

.group-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 8px;
}

.create-main-btn {
  background-color: #3b82f6;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 8px;
  font-weight: bold;
  cursor: pointer;
  margin-bottom: 20px;
  display: block;
  transition: background-color 0.2s;
}

.create-main-btn:hover {
  background-color: #2563eb;
}
</style>