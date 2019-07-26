<template>
  <div>
    <h1>{{ title }}</h1>
    <Error name="GenericError" context="FooBar" message="This is error content" />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Error from 'shared/vue/ErrorReporting/Error.vue';
import { Component, Prop } from "vue-property-decorator";

@Component({
  components: {
    Error
  }
})
export default class ErrorReport extends Vue {
  @Prop({ type: String, required: true }) readonly title: string;
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";
</style>
