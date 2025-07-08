<template>
  <v-dialog v-model="isOpen" max-width="500px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4">
        <span>{{ localProduct.pK_PRODUCT ? 'Editar Producto' : 'Agregar Producto' }}</span>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-container>
            <v-row>
              <v-col v-if="localProduct.pK_PRODUCT == null" cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localProduct.code" :maxlength="25"
                  counter="25" @keyup="uppercase" label="Código" required />
              </v-col>
              <v-col v-else cols="12" md="12" lg="12" xl="12">
                <v-text-field color="primary" variant="underlined" v-model="localProduct.code" :maxlength="25"
                  counter="25" @keyup="uppercase" label="Código" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localProduct.description"
                  :rules="[rules.required]" counter="50" :maxlength="50" @keyup="uppercase" label="Descripción"
                  required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localProduct.measurement"
                  :rules="[rules.required]" counter="15" :maxlength="15" @keyup="uppercase" label="Medida" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localProduct.material" counter="20"
                  :maxlength="20" @keyup="uppercase" label="Material" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localProduct.colour" :maxlength="20"
                  counter="20" @keyup="uppercase" label="Color" required />
              </v-col>
              <v-col v-if="localProduct.pK_PRODUCT == null" cols="12" md="12" lg="6" xl="6">
                <v-text-field color="primary" variant="underlined" v-model="localProduct.price"
                  onkeypress="return (event.charCode >= 48 && event.charCode <= 57)" :rules="[rules.required]"
                  counter="5" :maxlength="5" label="Precio" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-autocomplete color="primary" variant="underlined" :items="categories"
                  v-model="localProduct.pK_CATEGORY" item-title="categorY_NAME" item-value="pK_CATEGORY"
                  :rules="[rules.required]" no-data-text="No hay datos disponibles" label="Categoría" required />
              </v-col>
              <v-col cols="12" md="12" lg="6" xl="6">
                <v-autocomplete color="primary" variant="underlined" :items="brands" v-model="localProduct.pK_BRAND"
                  item-title="branD_NAME" item-value="pK_BRAND" :rules="[rules.required]"
                  no-data-text="No hay datos disponibles" label="Marca" required />
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-col xs12 sm12 md12 lg12 xl12>
        <v-card-actions>
          <v-btn color="blue" dark class="mb-2" elevation="4" @click="saveProduct" :disabled="!valid">Guardar</v-btn>
          <v-btn color="red" dark class="mb-2" elevation="4" @click="close">Cancelar</v-btn>
        </v-card-actions>
      </v-col>
    </v-card>
  </v-dialog>
</template>

<script>
import { mapGetters } from 'vuex';
import { useToast } from 'vue-toastification';
export default {
  props: {
    modelValue: {
      type: Boolean,
      required: true,
    },
    product: {
      type: Object,
      default: () => ({
        pK_PRODUCT: null,
        code: '',
        description: '',
        measurement: '',
        material: '',
        colour: '',
        pK_CATEGORY: null,
        pK_BRAND: null
      }),
    },
  },
  data() {
    return {
      isOpen: this.modelValue,
      valid: false,
      localProduct: { ...this.product },
      toast: useToast(),
      rules: {
        required: (value) => !!value || 'Este campo es requerido.',
      },
    };
  },
  computed: {
    ...mapGetters('category', ['categories']),
    ...mapGetters('brand', ['brands']),
  },
  watch: {
    modelValue(newValue) {
      //console.log('Diálogo abierto:', newValue);
      this.isOpen = newValue;
    },
    isOpen(newValue) {
      this.$emit('update:modelValue', newValue);
    },
    product: {
      handler(newProduct) {
        //console.log('Nuevo producto recibido:', newProduct);
        this.localProduct = { ...newProduct };
      },
      deep: true,
    },
  },
  mounted() {
    this.$store.dispatch('category/selectCategories');
    this.$store.dispatch('brand/selectBrands');
  },
  methods: {
    uppercase() {
      this.localProduct.code = this.localProduct.code.toUpperCase();
      this.localProduct.description = this.localProduct.description.toUpperCase();
      this.localProduct.measurement = this.localProduct.measurement.toUpperCase();
      this.localProduct.material = this.localProduct.material.toUpperCase();
      this.localProduct.colour = this.localProduct.colour.toUpperCase();
    },
    close() {
      this.isOpen = false;
    },
    async saveProduct() {
      if (this.$refs.form.validate()) {
        try {
          if (this.localProduct.pK_PRODUCT) {
            await this.$store.dispatch('product/updateProduct', { ...this.localProduct });
            this.toast.success('Producto actualizado con éxito!');
          } else {
            await this.$store.dispatch('product/createProduct', { ...this.localProduct });
            this.toast.success('Producto agregado con éxito!');
          }
          this.$emit('saved', { ...this.localProduct });
          this.close();
        } catch (error) {
          if (error.response) {
            this.toast.error('Error en generar el Producto.');
          }
        }
      }
    },
  },
};
</script>