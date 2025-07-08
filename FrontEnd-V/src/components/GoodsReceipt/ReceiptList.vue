<template>
  <div>
    <template v-if="!form">
      <v-card elevation="2">
        <v-data-table :headers="headers" :items="receipts" :search="search" :items-per-page-text="pages"
          :items-per-page-options="[5, 10, 20]" :items-per-page="5" :loading="loading"
          loading-text="Cargando... Espere por favor" page-text="{0}-{1} de {2}">
          <template v-slot:item="{ item }">
            <tr>
              <td>{{ item.code }}</td>
              <td>{{ item.datE_REGISTRATION }}</td>
              <td>{{ item.datE_PURCHASE }}</td>
              <td>{{ item.typE_RECEIPT }}</td>
              <td>{{ item.typE_DOCUMENT }}</td>
              <td>{{ item.documenT_NUMBER }}</td>
              <td>{{ item.companY_NAME }}</td>
              <td>{{ item.useR_NAME }}</td>
              <td>{{ item.storE_NAME }}</td>
              <td>
                <span :style="{ color: item.status === 'ACTIVO' ? 'green' : 'red' }">
                  {{ item.status }}
                </span>
              </td>
              <td>
                <v-btn color="blue" icon="tab" variant="text" @click="displayReceipt(item)" size="small"></v-btn>
                <template v-if="item.status == 'ACTIVO'">
                  <v-btn color="red" icon="block" variant="text" @click="openModal(item)" size="small"></v-btn>
                </template>
              </td>
            </tr>
          </template>
          <template v-slot:top>
            <v-toolbar>
              <v-toolbar-title>Gestión de Entradas</v-toolbar-title>
              <v-spacer></v-spacer>
              <v-text-field append-inner-icon="search" density="compact" label="Ingrese el código, proveedor o usuario" variant="solo" hide-details
                single-line v-model="search" @click:append-inner="fetchReceipt" class="elevation-1"></v-text-field>
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
      <ReceiptForm v-model="form" :receipt="selectedReceipt" :receiptDetails="selectedReceipt.details" @saved="fetchReceipts(this.$store.state.currentUser.pk_warehouse)" @close="form = false" />
    </template>
    <ReceiptAction v-model="modal" :receipt="selectedReceipt" @receipt-disabled="fetchReceipts(this.$store.state.currentUser.pk_warehouse)"/>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import ReceiptForm from './ReceiptForm.vue';
import ReceiptAction from './ReceiptAction.vue';
export default {
  components: {
    ReceiptForm,
    ReceiptAction,
  },
  data() {
    return {
      items: [5, 10, 25],
      pages: "Entradas por Página",
      search: null,
      form: false,
      modal: false,
      selectedReceipt: null,
    };
  },
  computed: {
    ...mapGetters('goodsreceipt', ['receipts', 'loading']),
    headers() {
      return [
        { title: 'Código', key: 'code', sortable: false },
        { title: 'Fecha de Registro', key: 'datE_REGISTRATION', sortable: false },
        { title: 'Fecha de Compra', key: 'datE_PURCHASE', sortable: false },
        { title: 'Tipo de Entrada', key: 'typE_RECEIPT', sortable: false },
        { title: 'Tipo de Comprobante', key: 'typE_DOCUMENT', sortable: false },
        { title: 'Número de Comprobante', key: 'receipT_NUMBER', sortable: false },
        { title: 'Proveedor', key: 'companY_NAME', sortable: false },
        { title: 'Usuario', key: 'useR_NAME', sortable: false },
        { title: 'Sucursal', key: 'storE_NAME', sortable: false },
        { title: 'Estado', key: 'status', sortable: false },
        { title: 'Acciones', key: 'actions', sortable: false },
      ];
    },
  },
  methods: {
    initialize() {
      this.$store.dispatch('goodsreceipt/fetchReceipts', this.$store.state.currentUser.pk_warehouse);
      this.search = null;
    },
    ...mapActions('goodsreceipt', ['fetchReceipts']),
    openForm() {
      this.selectedReceipt = {
        pK_RECEIPT: null, 
        code: '',
        datE_PURCHASE: '',
        datE_REGISTRATION: '',
        typE_RECEIPT: '',
        typE_DOCUMENT: '',
        documenT_NUMBER_NUMBER: '',
        annotations: '',
        pK_SUPPLIER: null,
        pK_USER: null,
        pK_STORE: null
      };
      this.form = true;
    },
    async displayReceipt(receipt) {
      this.selectedReceipt = { ...receipt };
      await this.$store.dispatch('goodsreceipt/fetchDetail', receipt.pK_RECEIPT);
      const details = this.$store.getters['goodsreceipt/detailsReceipt'];
      this.selectedReceipt.details = details || []
      this.form = true;
    },
    openModal(receipt) {
      this.selectedReceipt = { ...receipt };
      this.modal = true;
    },
    fetchReceipt() {
      this.$store.dispatch('goodsreceipt/fetchReceipt', {
        text: this.search,
        id: this.$store.state.currentUser.pk_warehouse
      });
    },
  },
  mounted() {
    this.fetchReceipts(this.$store.state.currentUser.pk_warehouse);
  },
};
</script>