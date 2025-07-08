<template>
  <v-dialog v-model="isOpen" max-width="400px" persistent>
    <v-card>
      <v-card-title class="bg-surface-light pt-4">Anular Item?</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        Estás a punto de Anular el ítem {{ localIssue.code }}.
      </v-card-text>
      <v-card-actions class="d-flex justify-space-between">
        <div class="d-flex">
          <v-btn color="blue" dark class="mr-2" elevation="4" @click="disabled">Anular</v-btn>
          <v-btn color="red" elevation="4" @click="close">Cancelar</v-btn>
        </div>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    modelValue: {
      type: Boolean,
      required: true,
    },
    issue: {
      type: Object,
      default: () => ({ pK_ISSUE: null, code: '' }),
    },
  },
  data() {
    return {
      isOpen: this.modelValue,
      valid: false,
      localIssue: { ...this.issue },
    };
  },
  watch: {
    modelValue(newValue) {
      this.isOpen = newValue;
    },
    issue: {
      handler(newIssue) {
        this.localIssue = { ...newIssue };
      },
      deep: true,
    },
  },
  methods: {
    close() {
      this.isOpen = false;
      this.$emit('update:modelValue', false);
    },
    async disabled() {
      try {
        await this.$store.dispatch('goodsissue/disabledIssue', this.localIssue.pK_ISSUE);
        this.$emit('issue-disabled');
        this.toast.success('Salida anulada con éxito!');
        this.close();
      } catch (error) {
        if (error.response) {
          this.toast.error('Error en anular la Salida.');
        }
      }
    },
  },
};
</script>