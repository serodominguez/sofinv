<template>
  <v-card elevation="2">
    <v-data-table :headers="headers" :items="users" :search="search" :items-per-page-text="pages"
      :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
      loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
      <template v-slot:item="{ item }">
        <tr>
          <td>{{ item.useR_NAME }}</td>
          <td>{{ item.names }}</td>
          <td>{{ item.lasT_NAMES }}</td>
          <td>{{ item.identification }}</td>
          <td>{{ item.phone }}</td>
          <td>{{ item.rolE_NAME }}</td>
          <td>{{ item.storE_NAME }}</td>
          <td>{{ item.creatioN_DATE }}</td>
          <td>{{ item.status }}</td>
          <td>
            <v-btn v-if="item.status =='ACTIVO'" color="blue" icon="edit" variant="text" @click="editUser(item)" size="small"></v-btn>
            <template v-if="item.status == 'INACTIVO'">
            <v-btn color="green" icon="check" variant="text" @click="openModal(item, 1)" size="small"></v-btn>
            </template>
            <template v-if="item.status == 'ACTIVO'">
              <v-btn color="red" icon="block" variant="text" @click="openModal(item, 2)" size="small"></v-btn>
            </template>
          </td>
        </tr>
      </template>
      <template v-slot:top>
        <v-toolbar>
          <v-toolbar-title>Gestión de Usuarios</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field append-inner-icon="search" density="compact" label="Ingrese el nombre, rol o sucursal" variant="solo" hide-details
            single-line v-model="search" @click:append-inner="fetchUser" class="elevation-1"></v-text-field>
          <v-card-actions>
            <v-btn @click="openForm" color="primary" elevation="4"> Agregar </v-btn>
          </v-card-actions>
        </v-toolbar>
      </template>
      <template v-slot:no-data>
        <v-btn color="primary" @click="initialize"> Resetear </v-btn>
      </template>
    </v-data-table>
  </v-card>
  <UserForm v-model="form" :user="selectedUser" @saved="fetchUsers" />
  <UserAction v-model="modal" :user="selectedUser" :action="action" @update:modelValue="modal = $event" />
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import UserForm from './UserForm.vue';
import UserAction from './UserAction.vue';
export default {
  components: {
    UserForm,
    UserAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Usuarios por Página",
      search: null,
      form: false,
      modal: false,
      selectedUser: null,
      action: 0,
    };
  },
  computed: {
    ...mapGetters('user', ['users', 'loading']),
    headers() {
      return [
        { title: 'Usuario', key: 'useR_NAME', sortable: false },
        { title: 'Nombres', key: 'names', sortable: false },
        { title: 'Apellidos', key: 'lasT_NAMES', sortable: false },
        { title: 'Carnet', key: 'identification', sortable: false },
        { title: 'Teléfono', key: 'phone', sortable: false },
        { title: 'Rol', key: 'rolE_NAME', sortable: false },
        { title: 'Sucursal', key: 'storE_NAME', sortable: false },
        { title: 'Agregado', key: 'creatioN_DATE', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('user/fetchUsers');
      this.search = null;
    },
    ...mapActions('user', ['fetchUsers']),
    openForm() {
      this.selectedUser = {
        pK_USER: null,
        useR_NAME: '',
        passworD_HASH: '',
        names: '',
        lasT_NAMES: '',
        identification: '',
        phone: '',
        pK_ROLE: '',
        pK_STORE: ''
      };
      this.form = true;
    },
    editUser(user) {
      this.selectedUser = { ...user };
      this.form = true;
    },
    openModal(user, action) {
      this.selectedUser = { ...user };
      this.action = action;
      this.modal = true;
    },
    fetchUser() {
      this.$store.dispatch('user/fetchUser', this.search);
    },
  },
  mounted() {
    this.fetchUsers();
  },
};
</script>