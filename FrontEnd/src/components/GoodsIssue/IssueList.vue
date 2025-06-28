<template>
  <div>
    <template v-if="!form">
      <v-card elevation="2">
        <v-data-table :headers="headers" :items="issues" :search="search" :items-per-page-text="pages"
          :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
          loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
          <template v-slot:item="{ item }">
            <tr>
              <td>{{ item.code }}</td>
              <td>{{ item.datE_REGISTRATION }}</td>
              <td>{{ item.datE_SALE }}</td>
              <td>{{ item.typE_ISSUE }}</td>
              <td>{{ item.typE_DOCUMENT }}</td>
              <td>{{ item.documenT_NUMBER }}</td>
              <td>{{ item.names }}</td>
              <td>{{ item.useR_NAME }}</td>
              <td>{{ item.storE_NAME }}</td>
              <td>
                <span :style="{ color: item.status === 'ACTIVO' ? 'green' : 'red' }">
                  {{ item.status }}
                </span>
              </td>
              <td>
                <v-btn color="blue" icon="tab" variant="text" @click="displayIssue(item)" size="small"></v-btn>
                <template v-if="item.status == 'ACTIVO'">
                  <v-btn color="red" icon="block" variant="text" @click="openModal(item)" size="small"></v-btn>
                </template>
              </td>
            </tr>
          </template>
          <template v-slot:top>
            <v-toolbar>
              <v-toolbar-title>Gestión de Salidas</v-toolbar-title>
              <v-spacer></v-spacer>
              <v-text-field append-inner-icon="search" density="compact" label="Ingrese el código, cliente o usuario" variant="solo" hide-details
                single-line v-model="search" @click:append-inner="fetchIssue" class="elevation-1"></v-text-field>
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
    </template>
    <template v-else>
      <IssueForm v-model="form" :issue="selectedIssue" :issueDetails="selectedIssue.details" @saved="fetchIssues(this.$store.state.currentUser.pk_warehouse)" @close="form = false" />
    </template>
    <IssueAction v-model="modal" :issue="selectedIssue" @issue-disabled="fetchIssues(this.$store.state.currentUser.pk_warehouse)"/>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import IssueForm from './IssueForm.vue';
import IssueAction from './IssueAction.vue';
export default {
  components: {
    IssueForm,
    IssueAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Salidas por Página",
      search: null,
      form: false,
      modal: false,
      selectedIssue: null,
    };
  },
  computed: {
    ...mapGetters('goodsissue', ['issues', 'loading']),
    headers() {
      return [
        { title: 'Código', key: 'code', sortable: false },
        { title: 'Fecha de Registro', key: 'datE_REGISTRATION', sortable: false },
        { title: 'Fecha de Venta', key: 'datE_SALE', sortable: false },
        { title: 'Tipo de Salida', key: 'typE_ISSUE', sortable: false },
        { title: 'Tipo de Comprobante', key: 'typE_DOCUMENT', sortable: false },
        { title: 'Número de Comprobante', key: 'issuE_NUMBER', sortable: false },
        { title: 'Cliente', key: 'names', sortable: false },
        { title: 'Usuario', key: 'useR_NAME', sortable: false },
        { title: 'Sucursal', key: 'storE_NAME', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('goodsissue/fetchIssues', this.$store.state.currentUser.pk_warehouse);
      this.search = null;
    },
    ...mapActions('goodsissue', ['fetchIssues']),
    openForm() {
      this.selectedIssue = {
        pK_ISSUE: null, 
        code: '',
        datE_SALE: '',
        datE_REGISTRATION: '',
        typE_ISSUE: '',
        typE_DOCUMENT: '',
        documenT_NUMBER_NUMBER: '',
        annotations: '',
        pK_CUSTOMER: null,
        pK_USER: null,
        pK_STORE: null
      };
      this.form = true;
    },
    async displayIssue(issue) {
      this.selectedIssue = { ...issue };
      await this.$store.dispatch('goodsissue/fetchDetail', issue.pK_ISSUE);
      const details = this.$store.getters['goodsissue/detailsIssue'];
      this.selectedIssue.details = details || []
      this.form = true;
    },
    openModal(issue) {
      this.selectedIssue = { ...issue };
      this.modal = true;
    },
    fetchIssue() {
      this.$store.dispatch('goodsissue/fetchIssue', {
        text: this.search,
        id: this.$store.state.currentUser.pk_warehouse
      });
    },
  },
  mounted() {
    this.fetchIssues(this.$store.state.currentUser.pk_warehouse);
  },
};
</script>