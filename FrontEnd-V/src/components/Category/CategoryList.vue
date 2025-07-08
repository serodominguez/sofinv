<template>
  <v-card elevation="2">
    <v-data-table :headers="headers" :items="categories" :search="search" :items-per-page-text="pages"
      :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
      loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
      <template v-slot:item="{ item }">
        <tr>
          <td>{{ item.categorY_NAME }}</td>
          <td>{{ item.creatioN_DATE }}</td>
          <td>{{ item.status }}</td>
          <td>
            <v-btn v-if="item.status =='ACTIVO'" color="blue" icon="edit" variant="text" @click="editCategory(item)" size="small"></v-btn>
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
          <v-toolbar-title>Gestión de Categorías</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field append-inner-icon="search" density="compact" label="Ingrese la categoría" variant="solo" hide-details
            single-line v-model="search" @click:append-inner="fetchCategory" class="elevation-1"></v-text-field>
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
  <CategoryForm v-model="form" :category="selectedCategory" @saved="fetchCategories" />
  <CategoryAction v-model="modal" :category="selectedCategory" :action="action" @update:modelValue="modal = $event" />
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import CategoryForm from './CategoryForm.vue';
import CategoryAction from './CategoryAction.vue';
export default {
  components: {
    CategoryForm,
    CategoryAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Categorías por Página",
      search: null,
      form: false,
      modal: false,
      selectedCategory: null,
      action: 0,
    };
  },
  computed: {
    ...mapGetters('category', ['categories', 'loading']),
    headers() {
      return [
        { title: 'Categoría', key: 'categorY_NAME', sortable: false },
        { title: 'Agregado', key: 'creatioN_DATE', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('category/fetchCategories');
      this.search = null;
    },
    ...mapActions('category', ['fetchCategories']),
    openForm() {
      this.selectedCategory = {
        pK_CATEGORY: null,
        categorY_NAME: ''
      };
      this.form = true;
    },
    editCategory(category) {
      this.selectedCategory = { ...category };
      this.form = true;
    },
    openModal(category, action) {
      this.selectedCategory = { ...category };
      this.action = action;
      this.modal = true;
    },
    fetchCategory() {
      this.$store.dispatch('category/fetchCategory', this.search);
    },
  },
  mounted() {
    this.fetchCategories();
  },
};
</script>