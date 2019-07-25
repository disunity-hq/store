<template>
  <div>
    <table class="table no-top-border">
      <thead>
        <tr>
          <th scope="col">Username</th>
          <th scope="col">Role</th>
          <th scope="col"></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="membership in members" v-bind:key="membership.username">
          <td>{{ membership.userName }}</td>
          <td>
            <div v-if="canManageRoles"></div>
            <span v-else>{{membership.role}}</span>
          </td>
          <td class="smallCell" v-if="canManageMembers">
            <button
              v-if="canManageRoles && membership.role !== 'Owner'"
              type="button"
              class="btn btn-primary"
              v-on:click="removeMember(membership.userName)"
            >
              <i class="fas fa-minus" />
            </button>
          </td>
        </tr>
        <tr v-if="addingMember">
          <td>
            <div class="form-group">
              <input
                type="text"
                class="form-control"
                placeholder="Enter Username..."
                v-model="userName"
              />
            </div>
          </td>
          <td>
            <div class="form-group">
              <select class="form-control" v-model="role">
                <option v-for="role in roles" v-bind:key="role">{{role}}</option>
              </select>
            </div>
          </td>
          <td class="smallCell">
            <button type="button" class="btn btn-primary" v-on:click="addMember">
              <i class="fas fa-save" />
            </button>
            <button type="button" class="btn btn-primary" v-on:click="addingMember = false">
              <i class="fas fa-trash" />
            </button>
          </td>
        </tr>
        <tr v-else-if="canManageRoles">
          <td></td>
          <td></td>
          <td key="member-add" id="ctrl-member-add">

            <button
              type="button"
              class="btn btn-primary"
              v-on:click="addingMember = !addingMember"
            >Add member</button>
          </td>
        </tr>
      </tbody>
    </table>
    <div id="orgMembersErrors"></div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";
import axios from "axios";
import "./syncfusion";
import ErrorReporter from "../ErrorReporter";

enum MemberRole {
  Member,
  Admin,
  Owner
}

interface IMembership {
  userName: string;
  role: string;
}

@Component
export default class OrgMembersTable extends Vue {
  members: IMembership[] = [];
  addingMember = false;

  roles = Object.keys(MemberRole).filter(k => isNaN(Number(k)));

  @Prop({ type: Boolean, default: false }) readonly canManageRoles: boolean;
  @Prop({ type: Boolean, default: false }) readonly canManageMembers: boolean;
  @Prop({ type: String, required: true }) readonly orgSlug: string;

  userName: string = "";
  role: string = "Member";

  readonly baseUrl = `/api/v1/orgs/${this.orgSlug}/members`;

  readonly errorReporter = new ErrorReporter("#orgMembersErrors");

  public async mounted() {
    try {
      const members = (await axios.get<IMembership[]>(this.baseUrl)).data;
      if (Array.isArray(members)) {
        this.members = members;
      }
    } catch (e) {
      console.error(e);
    }
  }

  public async addMember() {
    const membership: IMembership = {
      userName: this.userName,
      role: this.role
    };
    try {
      const response = await axios.post(this.baseUrl, membership);
      if (response.status === 204) {
        this.members.push(membership);
        this.addingMember = false;
        this.userName = "";
        this.role = "Member";
      } else {
        this.errorReporter.Empty();
        this.errorReporter.Set(response.data);
      }
    } catch (e) {
      console.error(e);
    }
  }

  public async removeMember(username: string) {
    try {
      const response = await axios.delete(`${this.baseUrl}/${username}`);

      if (response.status === 204) {
        this.members = this.members.filter(m => m.userName != username);
      } else {
        this.errorReporter.Empty();
        this.errorReporter.Set(response.data);
      }
    } catch (e) {
      console.error(e);
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";
#ctrl-member-add {
  text-align: right;
}
</style>
