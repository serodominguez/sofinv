<template>
  <v-card elevation="2">
    <v-data-table :headers="headers" :items="products" :search="search" :items-per-page-text="pages"
      :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
      loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
      <template v-slot:item="{ item }">
        <tr>
          <td>{{ item.code }}</td>
          <td>{{ item.description }}</td>
          <td>{{ item.measurement }}</td>
          <td>{{ item.material }}</td>
          <td>{{ item.colour }}</td>
          <td>{{ item.categorY_NAME }}</td>
          <td>{{ item.branD_NAME }}</td>
          <td>{{ item.creatioN_DATE }}</td>
          <td>{{ item.status }}</td>
          <td>
            <v-btn v-if="item.status =='ACTIVO'" color="blue" icon="edit" variant="text" @click="editProduct(item)" size="small"></v-btn>
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
          <v-toolbar-title>Gestión de Productos</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field append-inner-icon="search" density="compact" label="Ingrese el código, categoría o marca" variant="solo" hide-details
            single-line v-model="search" @click:append-inner="fetchProduct" class="elevation-1"></v-text-field>
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
  <ProductForm v-model="form" :product="selectedProduct" @saved="fetchProducts" />
  <ProductAction v-model="modal" :product="selectedProduct" :action="action" @update:modelValue="modal = $event" />
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import ProductForm from './ProductForm.vue';
import ProductAction from './ProductAction.vue';
export default {
  components: {
    ProductForm,
    ProductAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Productos por Página",
      search: null,
      form: false,
      modal: false,
      selectedProduct: null,
      action: 0,
    };
  },
  computed: {
    ...mapGetters('product', ['products', 'loading']),
    headers() {
      return [
        { title: 'Código', key: 'code', sortable: false },
        { title: 'Descripción', key: 'description', sortable: false },
        { title: 'Medida', key: 'measurement', sortable: false },
        { title: 'Material', key: 'material', sortable: false },
        { title: 'Color', key: 'colour', sortable: false },
        { title: 'Categoría', key: 'categoria', sortable: false },
        { title: 'Marca', key: 'Marca', sortable: false },
        { title: 'Agregado', key: 'creatioN_DATE', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('product/fetchProducts');
      this.search = null;
    },
    ...mapActions('product', ['fetchProducts']),
    openForm() {
      this.selectedProduct = {
        pK_PRODUCT: null,
        code: '',
        description: '',
        measurement: '',
        material: '',
        colour: '',
        pK_CATEGORY: null,
        pK_BRAND: null
      };
      this.form = true;
    },
    editProduct(product) {
      this.selectedProduct = { ...product };
      this.form = true;
    },
    openModal(product, action) {
      this.selectedProduct = { ...product };
      this.action = action;
      this.modal = true;
    },
    fetchProduct() {
      this.$store.dispatch('product/fetchProduct', this.search);
    },
  },
  mounted() {
    this.fetchProducts();
  },
};
</script>