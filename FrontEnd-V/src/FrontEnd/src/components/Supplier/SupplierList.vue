<template>
  <v-card elevation="2">
    <v-data-table :headers="headers" :items="suppliers" :search="search" :items-per-page-text="pages"
      :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
      loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
      <template v-slot:item="{ item }">
        <tr>
          <td>{{ item.companY_NAME }}</td>
          <td>{{ item.contact }}</td>
          <td>{{ item.phone }}</td>
          <td>{{ item.email }}</td>
          <td>{{ item.creatioN_DATE }}</td>
          <td>{{ item.status }}</td>
          <td>
            <v-btn v-if="item.status =='ACTIVO'" color="blue" icon="edit" variant="text" @click="editSupplier(item)" size="small"></v-btn>
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
          <v-toolbar-title>Gestión de Proveedores</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field append-inner-icon="search" density="compact" label="Ingrese el proveedor, contacto o teléfono" variant="solo" hide-details
            single-line v-model="search" @click:append-inner="fetchSupplier" class="elevation-1"></v-text-field>
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
  <SupplierForm v-model="form" :supplier="selectedSupplier" @saved="fetchSuppliers" />
  <SupplierAction v-model="modal" :supplier="selectedSupplier" :action="action" @update:modelValue="modal = $event" />
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import SupplierForm from './SupplierForm.vue';
import SupplierAction from './SupplierAction.vue';
export default {
  components: {
    SupplierForm,
    SupplierAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Proveedores por Página",
      search: null,
      form: false,
      modal: false,
      selectedSupplier: null,
      action: 0,
    };
  },
  computed: {
    ...mapGetters('supplier', ['suppliers', 'loading']),
    headers() {
      return [
        { title: 'Nombre Compañia', key: 'companY_NAME', sortable: false },
        { title: 'Contacto', key: 'contact', sortable: false },
        { title: 'Teléfono', key: 'phone', sortable: false },
        { title: 'Correo', key: 'email', sortable: false },
        { title: 'Agregado', key: 'creatioN_DATE', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('supplier/fetchSuppliers');
      this.search = null;
    },
    ...mapActions('supplier', ['fetchSuppliers']),
    openForm() {
      this.selectedSupplier = {
        pK_SUPPLIER: null,
        companY_NAME_NAME: '',
        contact: '',
        phone: '',
        email: ''
      };
      this.form = true;
    },
    editSupplier(supplier) {
      this.selectedSupplier = { ...supplier };
      this.form = true;
    },
    openModal(supplier, action) {
      this.selectedSupplier = { ...supplier };
      this.action = action;
      this.modal = true;
    },
    fetchSupplier() {
      this.$store.dispatch('supplier/fetchSupplier', this.search);
    },
  },
  mounted() {
    this.fetchSuppliers();
  },
};
</script>