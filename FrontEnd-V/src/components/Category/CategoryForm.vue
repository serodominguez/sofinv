<template>
  <v-dialog v-model="isOpen" max-width="500px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4">
        <span>{{ localCategory.pK_CATEGORY ? 'Editar Categoría' : 'Agregar Categoría' }}</span>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-container>
            <v-row>
              <v-col cols="12" md="12" lg="12" xl="12">
                <v-text-field color="primary" variant="underlined" v-model="localCategory.categorY_NAME"
                  :rules="[rules.required]" counter="40" :maxlength="40" @keyup="uppercase"
                  label="Nombre de la Categoría" required />
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-col xs12 sm12 md12 lg12 xl12>
        <v-card-actions>
          <v-btn color="blue" dark class="mb-2" elevation="4" @click="saveCategory" :disabled="!valid">Guardar</v-btn>
          <v-btn color="red" dark class="mb-2" elevation="4" @click="close">Cancelar</v-btn>
        </v-card-actions>
      </v-col>
    </v-card>
  </v-dialog>
</template>

<script>
import { useToast } from 'vue-toastification';
export default {
  props: {
    modelValue: {
      type: Boolean,
      required: true,
    },
    category: {
      type: Object,
      default: () => ({
        pK_CATEGORY: null,
        categorY_NAME: ''
      }),
    },
  },
  data() {
    return {
      isOpen: this.modelValue,
      valid: false,
      localCategory: { ...this.category },
      toast: useToast(),
      rules: {
        required: (value) => !!value || 'Este campo es requerido.',
      },
    };
  },
  watch: {
    modelValue(newValue) {
      this.isOpen = newValue;
    },
    isOpen(newValue) {
      this.$emit('update:modelValue', newValue);
    },
    category: {
      handler(newCategory) {
        this.localCategory = { ...newCategory };
      },
      deep: true,
    },
  },
  methods: {
    uppercase() {
      this.localCategory.categorY_NAME = this.localCategory.categorY_NAME.toUpperCase();
    },
    close() {
      this.isOpen = false;
    },
    async saveCategory() {
      if (this.$refs.form.validate()) {
        try {
          if (this.localCategory.pK_CATEGORY) {
            await this.$store.dispatch('category/updateCategory', { ...this.localCategory });
            this.toast.success('Categoría actualizada con éxito!');
          } else {
            await this.$store.dispatch('category/createCategory', { ...this.localCategory });
            this.toast.success('Categoría agregada con éxito!');
          }
          this.$emit('saved', { ...this.localCategory });
          this.close();
        } catch (error) {
          if (error.response) {
            this.toast.error('Error en generar la Categoría.');
          }
        }
      }
    },
  },
};
</script>