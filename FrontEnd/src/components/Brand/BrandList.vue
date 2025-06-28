<template>
  <v-card elevation="2">
    <v-data-table :headers="headers" :items="brands" :search="search" :items-per-page-text="pages"
      :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
      loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
      <template v-slot:item="{ item }">
        <tr>
          <td>{{ item.branD_NAME }}</td>
          <td>{{ item.creatioN_DATE }}</td>
          <td>{{ item.status }}</td>
          <td>
            <v-btn v-if="item.status =='ACTIVO'" color="blue" icon="edit" variant="text" @click="editBrand(item)" size="small"></v-btn>
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
          <v-toolbar-title>Gestión de Marcas</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field append-inner-icon="search" density="compact" label="Ingrese la marca" variant="solo" hide-details
            single-line v-model="search" @click:append-inner="searchBrand" class="elevation-1"></v-text-field>
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
  <BrandForm v-model="form" :brand="selectedBrand" @saved="fetchBrands" />
  <BrandAction v-model="modal" :brand="selectedBrand" :action="action" @update:modelValue="modal = $event" />
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import BrandForm from './BrandForm.vue';
import BrandAction from './BrandAction.vue';
export default {
  components: {
    BrandForm,
    BrandAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Marcas por Página",
      search: null,
      form: false,
      modal: false,
      selectedBrand: null,
      action: 0,
    };
  },
  computed: {
    ...mapGetters('brand', ['brands', 'loading']),
    headers() {
      return [
        { title: 'Marca', key: 'branD_NAME', sortable: false },
        { title: 'Agregado', key: 'creatioN_DATE', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('brand/fetchBrands');
      this.search = null;
    },
    ...mapActions('brand', ['fetchBrands']),
    openForm() {
      this.selectedBrand = {
        pK_BRAND: null,
        branD_NAME: ''
      };
      this.form = true;
    },
    editBrand(brand) {
      this.selectedBrand = { ...brand };
      this.form = true;
    },
    openModal(brand, action) {
      this.selectedBrand = { ...brand };
      this.action = action;
      this.modal = true;
    },
    searchBrand() {
      this.$store.dispatch('brand/fetchBrand', this.search);
    },
  },
  mounted() {
    this.fetchBrands();
  },
};
</script>