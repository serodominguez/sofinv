<template>
  <div>
    <v-toolbar>
      <v-toolbar-title>Salidas</v-toolbar-title>
      <v-divider class="mx-2" inset vertical></v-divider>
      <div class="font-weight-bold" style="font-size: 16px; margin-top: 4px; margin-right: 1620px;">{{
        this.localIssue.code }} </div>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-card v-if="isOpen">
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-row>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-select v-if="localIssue.pK_ISSUE == null" color="primary" variant="underlined"
                v-model="localIssue.typE_ISSUE" :items="issueTypes" label="Tipo de Salida"
                :rules="[rules.required]" @update:modelValue="updateDocuments" />
              <v-text-field v-else color="primary" variant="underlined" v-model="localIssue.typE_ISSUE"
                label="Tipo de Salida" readonly />
            </v-col>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-select v-if="localIssue.pK_ISSUE == null" color="primary" variant="underlined"
                v-model="localIssue.typE_DOCUMENT" :items="documentTypes" label="Tipo de Comprobante"
                :rules="[rules.required]" />
              <v-text-field v-else color="primary" variant="underlined" v-model="localIssue.typE_DOCUMENT"
                label="Tipo de Comprobante" readonly />
            </v-col>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-text-field v-if="localIssue.pK_ISSUE == null" color="primary" variant="underlined"
                v-model="localIssue.documenT_NUMBER" :rules="[rules.required]" counter="30" :maxlength="30"
                label="Número de Comprobante" @keyup="uppercase" />
              <v-text-field v-else color="primary" variant="underlined" v-model="localIssue.documenT_NUMBER"
                label="Número de Comprobante" readonly />
            </v-col>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-date-input v-if="localIssue.pK_ISSUE == null" locale="es" placeholder="dd/mm/yyyy"
                v-model="localIssue.datE_SALE" label="Fecha de Entrega" variant="underlined" prepend-icon=""
                :rules="[rules.required]" />
              <v-text-field v-else v-model="localIssue.datE_SALE" label="Fecha de Entrega" variant="underlined"
                readonly />
            </v-col>
            <v-col cols="6" md="2" lg="2" xl="2">
              <v-autocomplete v-if="localIssue.pK_ISSUE == null" color="primary" variant="underlined"
                :items="customers" v-model="localIssue.pK_CUSTOMER" item-title="names" item-value="pK_CUSTOMER"
                :rules="[rules.required]" no-data-text="No hay datos disponibles" label="Cliente" />
              <v-text-field v-else color="primary" variant="underlined" v-model="localIssue.names"
                label="Proveedor" readonly />
            </v-col>
            <v-col class="px-2" cols="6" md="2" lg="2" xl="2">
              <v-btn v-if="localIssue.pK_ISSUE == null" fab dark color="blue darken-1" class="mt-3"
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
              <td v-if="localIssue.pK_ISSUE == null"><v-text-field v-model.number="item.quantity"
                  variant="underlined"
                  onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"></v-text-field></td>
              <td v-else>{{ item.quantity }}</td>
              <td v-if="localIssue.pK_ISSUE == null"><v-text-field v-model="item.price" variant="underlined"
                  onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"></v-text-field></td>
              <td v-else>{{ item.price }}</td>
              <td>{{ thousandsFormat(item.quantity * item.price) }}</td>
              <td v-if="localIssue.pK_ISSUE == null">
                <v-btn color="red" icon="delete" variant="text" @click="removeProduct(item)" size="small"></v-btn>
              </td>
            </tr>
          </template>
        </v-data-table>
        <v-col cols="12" class="d-flex justify-end">
          <strong>Total Bs.</strong>{{ totalPrice }}
        </v-col>
        <v-col cols="12" md="12" lg="12" xl="12">
          <v-text-field color="primary" variant="underlined" label="Observaciones" counter="80" :maxlength="80"
            v-model="localIssue.annotations" @keyup="uppercase"></v-text-field>
        </v-col>
        <v-alert v-if="alert" type="warning" class="mt-3">
          No hay productos en la lista. Por favor, añade productos.
        </v-alert>
      </v-card-text>
      <v-col cols="12" md="12" lg="12" xl="12">
        <v-card-actions>
          <v-btn v-if="localIssue.pK_ISSUE == null" color="blue" dark class="mb-2" elevation="4"
            @click="saveIssue" :disabled="!valid">Guardar</v-btn>
          <v-btn v-else-if="localIssue.status == 'ACTIVO'" color="blue" dark class="mb-2" elevation="4" @click="printIssue">Imprimir</v-btn>
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
    issue: {
      type: Object,
      default: () => ({
        pK_ISSUE: null,
        code: '',
        datE_SALE: null,
        datE_REGISTRATION: '',
        typE_ISSUE: '',
        issuE_NUMBER: '',
        annotations: '',
        pK_CUSTOMER: null,
        pK_USER: null,
        pK_STORE: null
      }),
    },
    issueDetails: {
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
      localIssue: { ...this.issue },
      rules: {
        required: (value) => !!value || 'Este campo es requerido.',
      },
      documentTypes: [],
      typesOrders: ['ORDEN DE TRABAJO'],
      typesSales: ['FACTURA'],
      issueTypes: ['ARMADO', 'VENTA'],
    };
  },
  computed: {
    ...mapGetters('goodsissue', ['issues']),
    ...mapGetters('customer', ['customers']),
    headers() {
      const baseHeaders = [
        { title: 'Código', key: 'code', sortable: false },
        { title: 'Descripción', key: 'description', sortable: false },
        { title: 'Material', key: 'material', sortable: false },
        { title: 'Color', key: 'colour', sortable: false },
        { title: 'Categoría', key: 'categorY_NAME', sortable: false },
        { title: 'Marca', key: 'branD_NAME', sortable: false },
        { title: 'Cantidad', key: 'quantity', sortable: false },
        { title: 'Precio U.', key: 'price', sortable: false },
        { title: 'SubTotal', key: 'subtotal', sortable: false },
      ];
      if (this.localIssue.pK_ISSUE == null) {
        baseHeaders.push({ title: 'Borrar', key: 'actions', sortable: false });
      }

      return baseHeaders;
    },
    totalPrice() {
      const total = this.details.reduce((accumulator, item) => {
        return accumulator + (item.price * item.quantity);
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
    issue: {
      handler(newIssue) {
        this.localIssue = { ...newIssue };
        this.details = this.issueDetails;
      },
      deep: true,
    },
  },
  methods: {
    printIssue() {
      this.date = new Date().toLocaleString();
      const doc = new jsPDF('p', 'pt', 'letter');

      doc.setFontSize(10);
      doc.setFont('Helvetica', 'bold');
      doc.text("DOCUMENTO DE SALIDA", 210, 35);

      doc.setFontSize(9);
      doc.setFont('Helvetica', 'bold');
      doc.text("Sucursal: ", 20, 65);
      doc.text("Código: ", 20, 85);
      doc.text("Tipo de salida: ", 20, 105);
      doc.text("Fecha de venta: ", 20, 125);
      doc.text("Observaciones: ", 20, 145);
      doc.text("Fecha de impresión: ", 390, 65);
      doc.text("Fecha de salida: ", 390, 85);
      doc.text("Número de comprobante: ", 390, 105);
      doc.text("Cliente: ", 390, 125);

      doc.setFont('Helvetica', 'normal');
      doc.text(this.$store.state.currentUser.store_name, 63, 65);
      doc.text(this.localIssue.code, 57, 85);
      doc.text(this.localIssue.typE_ISSUE, 86, 105);
      doc.text(this.localIssue.datE_SALE, 92, 125);
      doc.text(this.localIssue.annotations, 90, 145);
      doc.text(this.date, 480, 65);
      doc.text(this.localIssue.datE_REGISTRATION, 464, 85);
      doc.text(this.localIssue.documenT_NUMBER, 502, 105);
      doc.text(this.localIssue.names, 425, 125);

      const rows = this.details.map(x => {
      const subtotals = x.quantity * x.price;
      return {
        code: x.code,
        description: x.description,
        material: x.material,
        colour: x.colour,
        categorY_NAME: x.categorY_NAME,
        branD_NAME: x.branD_NAME,
        quantity: x.quantity,
        price: x.price,
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
          { header: "Precio", dataKey: "price" },
          { header: "SubTotal", dataKey: "subtotals" }
        ],
        body: rows,
        startY: 160,
        margin: { horizontal: 20 },
        styles: {
          overflow: "linebreak",
          lineWidth: 0.5,
          lineColor: [0, 0, 0],
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
          price: { halign: 'right' },
          subtotals: { halign: 'right' }
        }
      })

      const total = this.totalPrice;
      let finalY = doc.lastAutoTable.finalY;
      doc.setFontSize(10);
      doc.setFont('Helvetica', 'bold');
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
        title: "Salida a bodega",
      });

      window.open(doc.output('bloburl'));
    },
    async saveIssue() {
      if (this.details.length === 0) {
        this.alert = true;
        return;
      }

      const isFormValid = this.$refs.form.validate();

      const requiredFields = [
        this.localIssue.typE_ISSUE,
        this.localIssue.typE_DOCUMENT,
        this.localIssue.datE_SALE,
        this.localIssue.documenT_NUMBER,
        this.localIssue.pK_CUSTOMER,
      ];

      const allFieldsValid = requiredFields.every(field => !!field);

      if (!isFormValid || !allFieldsValid) {
        return;
      }

      try {
        const issueData = {
          datE_SALE: this.localIssue.datE_SALE,
          typE_ISSUE: this.localIssue.typE_ISSUE,
          typE_DOCUMENT: this.localIssue.typE_DOCUMENT,
          documenT_NUMBER: this.localIssue.documenT_NUMBER,
          annotations: this.localIssue.annotations,
          pK_CUSTOMER: this.localIssue.pK_CUSTOMER,
          pK_USER: this.$store.state.currentUser.pk_user,
          pK_WAREHOUSE: this.$store.state.currentUser.pk_warehouse,
          details: this.details,
        };
        await this.$store.dispatch('goodsissue/createIssue', issueData);            
        this.toast.success('Salida agregada con éxito!');
        this.$emit('saved', { ...this.localIssue });
        this.close();
      } catch (error) {
        if (error.response) {
            this.toast.error('Error en generar la Salida.');
          }
      }
    },
    updateDocuments() {
      if (this.localIssue.typE_ISSUE === 'VENTA') {
        this.localIssue.typE_DOCUMENT = null,
          this.documentTypes = this.typesSales;
      } else if (this.localIssue.typE_ISSUE === 'ARMADO') {
        this.localIssue.typE_DOCUMENT = null,
          this.documentTypes = this.typesOrders;
      } else {
        this.documentTypes = [];
      }
    },
    uppercase() {
      this.localIssue.documenT_NUMBER = this.localIssue.documenT_NUMBER.toUpperCase();
      this.localIssue.annotations = this.localIssue.annotations.toUpperCase();
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
          price: 0,
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
    this.details = [...this.issueDetails];
    this.$store.dispatch('customer/selectCustomers');

    if (this.localIssue.pK_ISSUE == null) {
      this.localIssue.datE_SALE = null;
      this.updateDocuments();
    }
  }
};
</script>