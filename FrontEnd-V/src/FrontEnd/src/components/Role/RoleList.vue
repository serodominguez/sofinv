<template>
  <v-card elevation="2">
    <v-data-table :headers="headers" :items="roles" :search="search" :items-per-page-text="pages"
      :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
      loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
      <template v-slot:item="{ item }">
        <tr>
          <td>{{ item.rolE_NAME }}</td>
          <td>{{ item.creatioN_DATE }}</td>
          <td>{{ item.status }}</td>
        </tr>
      </template>
      <template v-slot:top>
        <v-toolbar>
          <v-toolbar-title>Gestión de Roles</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-text-field append-inner-icon="search" density="compact" label="Ingrese el rol" variant="solo" hide-details
            single-line v-model="search" class="elevation-1"></v-text-field>
          <v-card-actions>
          </v-card-actions>
        </v-toolbar>
      </template>
      <template v-slot:no-data>
        <v-btn color="primary" @click="initialize"> Resetear </v-btn>
      </template>
    </v-data-table>
  </v-card>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  data() {
    return {
      items: [5, 10, 25],
      pages: "Roles por Página",
      search: null,
    };
  },
  computed: {
    ...mapGetters('role', ['roles', 'loading']),
    headers() {
      return [
        { title: 'Rol', key: 'rolE_NAME', sortable: false },
        { title: 'Agregado', key: 'creatioN_DATE', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('role/fetchRoles');
      this.search = null;
    },
  },
  mounted() {
    this.initialize();
  },
};
</script>