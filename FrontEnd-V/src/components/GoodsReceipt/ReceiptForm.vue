<template>
  <div>
    <v-toolbar>
      <v-toolbar-title>Entradas</v-toolbar-title>
      <v-divider class="mx-2" inset vertical></v-divider>
      <div class="font-weight-bold" style="font-size: 16px; margin-top: 4px; margin-right: 1620px;">{{
        this.localReceipt.code }} </div>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card v-if="isOpen">
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-row>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-select v-if="localReceipt.pK_RECEIPT == null" color="primary" variant="underlined"
                v-model="localReceipt.typE_RECEIPT" :items="receiptTypes" label="Tipo de Entrada"
                :rules="[rules.required]" @update:modelValue="updateDocuments" />
              <v-text-field v-else color="primary" variant="underlined" v-model="localReceipt.typE_RECEIPT"
                label="Tipo de Entrada" readonly />
            </v-col>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-select v-if="localReceipt.pK_RECEIPT == null" color="primary" variant="underlined"
                v-model="localReceipt.typE_DOCUMENT" :items="documentTypes" label="Tipo de Comprobante"
                :rules="[rules.required]" />
              <v-text-field v-else color="primary" variant="underlined" v-model="localReceipt.typE_DOCUMENT"
                label="Tipo de Comprobante" readonly />
            </v-col>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-text-field v-if="localReceipt.pK_RECEIPT == null" color="primary" variant="underlined"
                v-model="localReceipt.documenT_NUMBER" :rules="[rules.required]" counter="30" :maxlength="30"
                label="Número de Comprobante" @keyup="uppercase" />
              <v-text-field v-else color="primary" variant="underlined" v-model="localReceipt.documenT_NUMBER"
                label="Número de Comprobante" readonly />
            </v-col>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-date-input v-if="localReceipt.pK_RECEIPT == null" locale="es" placeholder="dd/mm/yyyy"
                v-model="localReceipt.datE_PURCHASE" label="Fecha de Adquisición" variant="underlined" prepend-icon=""
                :rules="[rules.required]" />
              <v-text-field v-else v-model="localReceipt.datE_PURCHASE" label="Fecha de Adquisición" variant="underlined"
                readonly />
            </v-col>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-autocomplete v-if="localReceipt.pK_RECEIPT == null" color="primary" variant="underlined"
                :items="suppliers" v-model="localReceipt.pK_SUPPLIER" item-title="companY_NAME" item-value="pK_SUPPLIER"
                :rules="[rules.required]" no-data-text="No hay datos disponibles" label="Proveedor" />
              <v-text-field v-else color="primary" variant="underlined" v-model="localReceipt.companY_NAME"
                label="Proveedor" readonly />
            </v-col>
            <v-col class="px-2" cols="6" md="2" lg="2" xl="2">
              <v-btn v-if="localReceipt.pK_RECEIPT == null" fab dark color="blue darken-1" class="mt-3"
                @click="openModal">
                <v-icon dark>list</v-icon>
              </v-btn>
            </v-col>
          </v-row>
        </v-form>
        <v-divider></v-divider>
        <v-data-table :headers="headers" :items="details" class="elevation-1" hide-default-footer>
          <template v-slot:item="{ item }">
            <tr>
              <td>{{ item.code }}</td>
              <td>{{ item.description }}</td>
              <td>{{ item.material }}</td>
              <td>{{ item.colour }}</td>
              <td>{{ item.categorY_NAME }}</td>
              <td>{{ item.branD_NAME }}</td>
              <td v-if="localReceipt.pK_RECEIPT == null"><v-text-field v-model.number="item.quantity"
                  variant="underlined"
                  onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"></v-text-field></td>
              <td v-else>{{ item.quantity }}</td>
              <td v-if="localReceipt.pK_RECEIPT == null"><v-text-field v-model="item.cost" variant="underlined"
                  onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"></v-text-field></td>
              <td v-else>{{ item.cost }}</td>
              <td>{{ thousandsFormat(item.quantity * item.cost) }}</td>
              <td v-if="localReceipt.pK_RECEIPT == null">
                <v-btn color="red" icon="delete" variant="text" @click="removeProduct(item)" size="small"></v-btn>
              </td>
            </tr>
          </template>
        </v-data-table>
        <v-col cols="12" class="d-flex justify-end">
          <strong>Total Bs.</strong>{{ totalCost }}
        </v-col>
        <v-col cols="12" md="12" lg="12" xl="12">
          <v-text-field color="primary" variant="underlined" label="Observaciones" counter="80" :maxlength="80"
            v-model="localReceipt.annotations" @keyup="uppercase"></v-text-field>
        </v-col>
        <v-alert v-if="alert" type="warning" class="mt-3">
          No hay productos en la lista. Por favor, añade productos.
        </v-alert>
      </v-card-text>
      <v-col cols="12" md="12" lg="12" xl="12">
        <v-card-actions>
          <v-btn v-if="localReceipt.pK_RECEIPT == null" color="blue" dark class="mb-2" elevation="4"
            @click="saveReceipt" :disabled="!valid">Guardar</v-btn>
          <v-btn v-else-if="localReceipt.status == 'ACTIVO'" color="blue" dark class="mb-2" elevation="4" @click="printReceipt">Imprimir</v-btn>
          <v-btn color="red" dark class="mb-2" elevation="4" @click="close">Cancelar</v-btn>
        </v-card-actions>
      </v-col>
    </v-card>
    <ProductModal v-model="modal" @update:modelValue="modal = $event" @check-product="handleCheckProduct" :warningMessage="warningMessage" @clear-warning="warningMessage = ''" />
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import ProductModal from '../Product/ProductModal.vue';
import { jsPDF } from 'jspdf'
import { autoTable } from 'jspdf-autotable'
import { useToast } from 'vue-toastification';
export default {
  components: {
    ProductModal,
  },
  props: {
    modelValue: {
      type: Boolean,
      default: false,
    },
    receipt: {
      type: Object,
      default: () => ({
        pK_RECEIPT: null,
        code: '',
        datE_PURCHASE: null,
        datE_REGISTRATION: '',
        typE_RECEIPT: '',
        receipT_NUMBER: '',
        annotations: '',
        pK_SUPPLIER: null,
        pK_USER: null,
        pK_STORE: null
      }),
    },
    receiptDetails: {
      type: Array,
      default: () => []
    }
  },
  emits: ['update:modelValue', 'saved', 'close'],
  data() {
    return {
      details: [],
      itemCounter: 1,
      items: [5, 10, 25],
      pages: "Productos por Página",
      isOpen: this.modelValue,
      alert: false,
      valid: false,
      modal: false,
      warningMessage: '',
      toast: useToast(),
      localReceipt: { ...this.receipt },
      rules: {
        required: (value) => !!value || 'Este campo es requerido.',
      },
      documentTypes: [],
      typesPurchases: ['FACTURA', 'RECIBO'],
      typesImports: ['DUI'],
      receiptTypes: ['ADQUISICIÓN', 'IMPORTACIÓN'],
    };
  },
  computed: {
    ...mapGetters('goodsreceipt', ['receipts']),
    ...mapGetters('supplier', ['suppliers']),
    headers() {
      const baseHeaders = [
        { title: 'Código', key: 'code', sortable: false },
        { title: 'Descripción', key: 'description', sortable: false },
        { title: 'Material', key: 'material', sortable: false },
        { title: 'Color', key: 'colour', sortable: false },
        { title: 'Categoría', key: 'categorY_NAME', sortable: false },
        { title: 'Marca', key: 'branD_NAME', sortable: false },
        { title: 'Cantidad', key: 'quantity', sortable: false },
        { title: 'Costo U.', key: 'cost', sortable: false },
        { title: 'SubTotal', key: 'subtotal', sortable: false },
      ];
      if (this.localReceipt.pK_RECEIPT == null) {
        baseHeaders.push({ title: 'Borrar', key: 'actions', sortable: false });
      }

      return baseHeaders;
    },
    totalCost() {
      const total = this.details.reduce((accumulator, item) => {
        return accumulator + (item.cost * item.quantity);
      }, 0); 
      return total.toLocaleString('en-US');
    }
  },
  watch: {
    modelValue(newValue) {
      this.isOpen = newValue;
    },
    isOpen(newValue) {
      this.$emit('update:modelValue', newValue);
    },
    receipt: {
      handler(newReceipt) {
        this.localReceipt = { ...newReceipt };
        this.details = this.receiptDetails;
      },
      deep: true,
    },
  },
  methods: {
    printReceipt() {
      this.date = new Date().toLocaleString();
      const doc = new jsPDF('p', 'pt', 'letter');

      // Agregar la imagen
      //const imgData = 'data:image/png;base64,...'; // Reemplaza esto con tu imagen en base64 o una URL
      //doc.addImage(imgData, 'PNG', 20, 20, 50, 50); // Ajusta las coordenadas y el tamaño según sea necesario

      doc.setFontSize(10);
      doc.setFont('Helvetica', 'bold');
      doc.text("DOCUMENTO DE ENTRADA", 210, 35);
      //doc.text(this.$store.state.currentUser.store_name, 20, 50);

      doc.setFontSize(9);
      doc.setFont('Helvetica', 'bold');
      doc.text("Sucursal: ", 20, 65);
      doc.text("Código: ", 20, 85);
      doc.text("Tipo de entrada: ", 20, 105);
      doc.text("Fecha de compra: ", 20, 125);
      doc.text("Observaciones: ", 20, 145);
      doc.text("Fecha de impresión: ", 390, 65);
      doc.text("Fecha de ingreso: ", 390, 85);
      doc.text("Número de comprobante: ", 390, 105);
      doc.text("Proveedor: ", 390, 125);

      doc.setFont('Helvetica', 'normal');
      doc.text(this.$store.state.currentUser.store_name, 63, 65);
      doc.text(this.localReceipt.code, 57, 85);
      doc.text(this.localReceipt.typE_RECEIPT, 92, 105);
      doc.text(this.localReceipt.datE_PURCHASE, 99, 125);
      doc.text(this.localReceipt.annotations, 90, 145);
      doc.text(this.date, 480, 65);
      doc.text(this.localReceipt.datE_REGISTRATION, 470, 85);
      doc.text(this.localReceipt.documenT_NUMBER, 503, 105);
      doc.text(this.localReceipt.companY_NAME, 440, 125);

      const rows = this.details.map(x => {
      const subtotals = x.quantity * x.cost;
      return {
        code: x.code,
        description: x.description,
        material: x.material,
        colour: x.colour,
        categorY_NAME: x.categorY_NAME,
        branD_NAME: x.branD_NAME,
        quantity: x.quantity,
        cost: x.cost,
        subtotals: subtotals.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 })
      };
    });

      autoTable(doc, {
        columns: [
          { header: "Código", dataKey: "code" },
          { header: "Descripción", dataKey: "description" },
          { header: "Material", dataKey: "material" },
          { header: "Color", dataKey: "colour" },
          { header: "Categoría", dataKey: "categorY_NAME" },
          { header: "Marca", dataKey: "branD_NAME" },
          { header: "Cantidad", dataKey: "quantity" },
          { header: "Costo", dataKey: "cost" },
          { header: "SubTotal", dataKey: "subtotals" }
        ],
        body: rows,
        startY: 160,
        margin: { horizontal: 20 },
        styles: {
          overflow: "linebreak",
          lineWidth: 0.5,
          lineColor: [0, 0, 0],
          //halign: 'center' 
        },
        bodyStyles: {
          textColor: [33, 33, 33],
          fillColor: [255, 255, 255],
          font: 'helvetica',
          valign: "top",
          fontSize: 8
        },
        headStyles: {
          lineWidth: 0.5,
          lineColor: [0, 0, 0],
          halign: 'center'
        },
        footStyles: {
          fillColor: [255, 255, 255],
          textColor: [33, 33, 33],
          halign: 'left'
        },
        showHead: "everyPage",
        showFoot: "lastPage",
        alternateRowStyles: {},
        columnStyles: {
          quantity: { halign: 'right' },
          cost: { halign: 'right' },
          subtotals: { halign: 'right' }
        }
      })

      const total = this.totalCost;
      let finalY = doc.lastAutoTable.finalY;
      doc.setFontSize(10);
      doc.setFont('Helvetica', 'bold');
      //doc.text(`Total Bs. ${total}`, 510, finalY + 20);
      doc.text('Total Bs.', 20, finalY + 20);
      doc.setFont('Helvetica', 'normal');
      doc.text(total, 65, finalY + 20);

      const pageCount = doc.internal.getNumberOfPages();
      doc.setFontSize(10);
      doc.setFont('Helvetica', 'normal');
      for (let i = 1; i <= pageCount; i++) {
        doc.setPage(i);
        doc.text('Página ' + String(i) + ' de ' + String(pageCount), 525, 760);
      }

      doc.setProperties({
        title: "Entrada a bodega",
      });

      window.open(doc.output('bloburl'));
    },
    async saveReceipt() {
      if (this.details.length === 0) {
        this.alert = true;
        return;
      }

      const isFormValid = this.$refs.form.validate();

      const requiredFields = [
        this.localReceipt.typE_RECEIPT,
        this.localReceipt.typE_DOCUMENT,
        this.localReceipt.datE_PURCHASE,
        this.localReceipt.documenT_NUMBER,
        this.localReceipt.pK_SUPPLIER,
      ];

      const allFieldsValid = requiredFields.every(field => !!field);

      if (!isFormValid || !allFieldsValid) {
        return;
      }

      try {
        const receiptData = {
          datE_PURCHASE: this.localReceipt.datE_PURCHASE,
          typE_RECEIPT: this.localReceipt.typE_RECEIPT,
          typE_DOCUMENT: this.localReceipt.typE_DOCUMENT,
          documenT_NUMBER: this.localReceipt.documenT_NUMBER,
          annotations: this.localReceipt.annotations,
          pK_SUPPLIER: this.localReceipt.pK_SUPPLIER,
          pK_USER: this.$store.state.currentUser.pk_user,
          pK_WAREHOUSE: this.$store.state.currentUser.pk_warehouse,
          details: this.details,
        };
        await this.$store.dispatch('goodsreceipt/createReceipt', receiptData);            
        this.toast.success('Entrada agregada con éxito!');
        this.$emit('saved', { ...this.localReceipt });
        this.close();
      } catch (error) {
        if (error.response) {
            this.toast.error('Error en generar la Entrada.');
          }
      }
    },
    updateDocuments() {
      if (this.localReceipt.typE_RECEIPT === 'ADQUISICIÓN') {
        this.localReceipt.typE_DOCUMENT = null,
          this.documentTypes = this.typesPurchases;
      } else if (this.localReceipt.typE_RECEIPT === 'IMPORTACIÓN') {
        this.localReceipt.typE_DOCUMENT = null,
          this.documentTypes = this.typesImports;
      } else {
        this.documentTypes = [];
      }
    },
    uppercase() {
      this.localReceipt.documenT_NUMBER = this.localReceipt.documenT_NUMBER.toUpperCase();
      this.localReceipt.annotations = this.localReceipt.annotations.toUpperCase();
    },
    handleCheckProduct(product) {
      const exists = this.details.some(item => item.pK_PRODUCT === product.pK_PRODUCT);

      if (!exists) {
        this.details.push({
          pK_PRODUCT: product.pK_PRODUCT,
          code: product.code,
          description: product.description,
          material: product.material,
          colour: product.colour,
          categorY_NAME: product.categorY_NAME,
          branD_NAME: product.branD_NAME,
          quantity: 0,
          cost: 0,
          subtotal: 0
        });
        this.warningMessage = '';
      } else {
        this.warningMessage = 'El producto ya está en la lista.';
      }
    },
    removeProduct(details) {
      const index = this.details.indexOf(details);
      if (index > -1) {
        this.details.splice(index, 1);
      }
    },
    thousandsFormat(value) {
      return value.toLocaleString('en-US');
  },
    openModal() {
      this.modal = true;
    },
    close() {
      this.isOpen = false;
      this.warningMessage = '';
      this.$emit('close');
    },
  },
  mounted() {
    this.details = [...this.receiptDetails];
    this.$store.dispatch('supplier/selectSuppliers');

    if (this.localReceipt.pK_RECEIPT == null) {
      this.localReceipt.datE_PURCHASE = null;
      this.updateDocuments();
    } /* else {
      const parts = this.localReceipt.datE_PURCHASE.split('/');
      if (parts.length === 3) {
        const day = parseInt(parts[0], 10);
        const month = parseInt(parts[1], 10) - 1;
        const year = parseInt(parts[2], 10);
        const date = new Date(year, month, day);
        this.localReceipt.datE_PURCHASE = date;
      } else {
        console.error('Formato de fecha no válido:', this.localReceipt.datE_PURCHASE);
      }
    } */
  }
};
</script>