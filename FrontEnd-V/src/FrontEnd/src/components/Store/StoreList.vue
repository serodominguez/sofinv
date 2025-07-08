<template>
  <v-card elevation="2">
    <v-data-table :headers="headers" :items="stores" :search="search" :items-per-page-text="pages"
      :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
      loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
      <template v-slot:item="{ item }">
        <tr>
          <td>{{ item.storE_NAME }}</td>
          <td>{{ item.manager }}</td>
          <td>{{ item.address }}</td>
          <td>{{ item.phone }}</td>
          <td>{{ item.city }}</td>
          <td>{{ item.email }}</td>
          <td>{{ item.type }}</td>
          <td>{{ item.creatioN_DATE }}</td>
          <td>{{ item.status }}</td>
          <td>
            <v-btn v-if="item.status =='ACTIVO'" color="blue" icon="edit" variant="text" @click="editStore(item)" size="small"></v-btn>
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
          <v-toolbar-title>Gestión de Sucursales</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field append-inner-icon="search" density="compact" label="Ingrese la sucursal, encargado o ciudad" variant="solo" hide-details
            single-line v-model="search" @click:append-inner="fetchStore" class="elevation-1"></v-text-field>
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
  <StoreForm v-model="form" :store="selectedStore" @saved="fetchStores" />
  <StoreAction v-model="modal" :store="selectedStore" :action="action" @update:modelValue="modal = $event" />
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import StoreForm from './StoreForm.vue';
import StoreAction from './StoreAction.vue';
export default {
  components: {
    StoreForm,
    StoreAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Sucursales por Página",
      search: null,
      form: false,
      modal: false,
      selectedStore: null,
      action: 0,
    };
  },
  computed: {
    ...mapGetters('store', ['stores', 'loading']),
    headers() {
      return [
        { title: 'Sucursal', key: 'storE_NAME', sortable: false },
        { title: 'Encargado', key: 'manager', sortable: false },
        { title: 'Dirección', key: 'address', sortable: false },
        { title: 'Teléfono', key: 'phone', sortable: false },
        { title: 'Ciudad', key: 'city', sortable: false },
        { title: 'Correo', key: 'email', sortable: false },
        { title: 'Tipo', key: 'type', sortable: false },
        { title: 'Agregado', key: 'creatioN_DATE', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('store/fetchStores');
      this.search = null;
    },
    ...mapActions('store', ['fetchStores']),
    openForm() {
      this.selectedStore = {
        pK_STORE: null,
        storE_NAME: '',
        manager: '',
        address: '',
        phone: '',
        city: '',
        email: ''
      };
      this.form = true;
    },
    editStore(store) {
      this.selectedStore = { ...store };
      this.form = true;
    },
    openModal(store, action) {
      this.selectedStore = { ...store };
      this.action = action;
      this.modal = true;
    },
    fetchStore() {
      this.$store.dispatch('store/fetchStore', this.search);
    },
  },
  mounted() {
    this.fetchStores();
  },
};
</script>