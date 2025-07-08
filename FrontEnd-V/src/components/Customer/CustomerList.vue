<template>
  <v-card elevation="2">
    <v-data-table :headers="headers" :items="customers" :search="search" :items-per-page-text="pages"
      :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
      loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
      <template v-slot:item="{ item }">
        <tr>
          <td>{{ item.names }}</td>
          <td>{{ item.lasT_NAMES }}</td>
          <td>{{ item.identification }}</td>
          <td>{{ item.phone }}</td>
          <td>{{ item.creatioN_DATE }}</td>
          <td>{{ item.status }}</td>
          <td>
            <v-btn v-if="item.status =='ACTIVO'" color="blue" icon="edit" variant="text" @click="editCustomer(item)" size="small"></v-btn>
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
          <v-toolbar-title>Gestión de Clientes</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field append-inner-icon="search" density="compact" label="Ingrese el nombre, apellido o carnet" variant="solo" hide-details
            single-line v-model="search" @click:append-inner="searchCustomer" class="elevation-1"></v-text-field>
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
  <CustomerForm v-model="form" :customer="selectedCustomer" @saved="fetchCustomers" />
  <CustomerAction v-model="modal" :customer="selectedCustomer" :action="action" @update:modelValue="modal = $event" />
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import CustomerForm from './CustomerForm.vue';
import CustomerAction from './CustomerAction.vue';
export default {
  components: {
    CustomerForm,
    CustomerAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Clientes por Página",
      search: null,
      form: false,
      modal: false,
      selectedCustomer: null,
      action: 0,
    };
  },
  computed: {
    ...mapGetters('customer', ['customers', 'loading']),
    headers() {
      return [
        { title: 'Nombres', key: 'names', sortable: false },
        { title: 'Apellidos', key: 'lasT_NAMES', sortable: false },
        { title: 'Carnet', key: 'identification', sortable: false },
        { title: 'Teléfono', key: 'phone', sortable: false },
        { title: 'Agregado', key: 'creatioN_DATE', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('customer/fetchCustomers');
      this.search = null;
    },
    ...mapActions('customer', ['fetchCustomers']),
    openForm() {
      this.selectedCustomer = {
        pK_CUSTOMER: null,
        names: '',
        lasT_NAMES: '',
        identification: '',
        phone: ''
      };
      this.form = true;
    },
    editCustomer(customer) {
      this.selectedCustomer = { ...customer };
      this.form = true;
    },
    openModal(customer, action) {
      this.selectedCustomer = { ...customer };
      this.action = action;
      this.modal = true;
    },
    searchCustomer() {
      this.$store.dispatch('customer/fetchCustomer', this.search);
    },
  },
  mounted() {
    this.fetchCustomers();
  },
};
</script>