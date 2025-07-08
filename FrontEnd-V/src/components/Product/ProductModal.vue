<template>
  <v-dialog v-model="isOpen" max-width="1200px" persistent>
    <v-card elevation="2">
      <v-card-title class="bg-surface-light pt-4">
        <span>Seleccione el Producto</span>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-row justify="center">
          <v-col cols="12" md="8" lg="8" xl="8" class="mb-2">
            <v-text-field append-inner-icon="search" density="compact"
              label="Ingrese el código, precio, categoría o la marca" variant="solo" hide-details single-line
              v-model="search" @click:append-inner="searchProduct" class="elevation-1" @keyup.enter="searchProduct"
              @keyup="uppercase"></v-text-field>
          </v-col>
        </v-row>
        <v-data-table :headers="headers" :items="warehouses" :search="search" :items-per-page-text="pages"
          :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
          loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
          <template v-slot:item="{ item }">
            <tr>
              <td>{{ item.code }}</td>
              <td>{{ item.description }}</td>
              <td>{{ item.material }}</td>
              <td>{{ item.colour }}</td>
              <td>{{ item.stock }}</td>
              <td>{{ item.price }}</td>
              <td>{{ item.categorY_NAME }}</td>
              <td>{{ item.branD_NAME }}</td>
              <td>
                <v-btn color="blue" icon="add" variant="text" @click="addProduct(item)" size="small"></v-btn>
              </td>
            </tr>
          </template>
        </v-data-table>
        <v-alert v-if="warningMessage" type="warning" class="mt-3">
          {{ warningMessage }}
        </v-alert>
      </v-card-text>
      <v-col>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="red" dark class="mb-2" elevation="4" @click="close">Cerrar</v-btn>
        </v-card-actions>
      </v-col>
    </v-card>
  </v-dialog>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
export default {
  props: {
    modelValue: {
      type: Boolean,
      required: true,
    },
    warningMessage: {
    type: String,
    default: '',
  },
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Productos por Página",
      search: null,
      isOpen: this.modelValue,
    };
  },
  watch: {
    modelValue(newValue) {
      this.isOpen = newValue;
    },
  },
  computed: {
    ...mapGetters('warehouse', ['warehouses', 'loading']),
    headers() {
      return [
        { title: 'Código', key: 'code' },
        { title: 'Descripción', key: 'description', sortable: false },
        { title: 'Material', key: 'material', sortable: false },
        { title: 'Color', key: 'colour', sortable: false },
        { title: 'Cantidad', key: 'stock', sortable: false },
        { title: 'Precio', key: 'price', sortable: false },
        { title: 'Categoría', key: 'categorY_NAME', sortable: false },
        { title: 'Marca', key: 'branD_NAME', sortable: false },
        { title: 'Agregar', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    ...mapActions('warehouse', ['selectProducts']),
    searchProduct() {
      this.$store.dispatch('warehouse/selectProducts', {
        text: this.search,
        id: this.$store.state.currentUser.pk_warehouse
      });
    },
    addProduct(item) {
      this.$emit('check-product', item);
    },
    close() {
      this.isOpen = false;
      this.search = null;
      this.$emit('clear-warning');
      this.$store.commit('warehouse/SET_CLEAR');
      this.$emit('update:modelValue', false);
    },
    uppercase() {
      this.search = this.search.toUpperCase();
    },
  },
};
</script>